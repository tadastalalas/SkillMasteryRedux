using System;
using System.Reflection;
using HarmonyLib;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using SkillMasteryRedux.UI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Perk tooltip shows the mastery level | gate: IsEnabledFor + IsCandidate | Postfix: wrap the hint provider once the VM exists
[HarmonyPatch(typeof(PerkVM), MethodType.Constructor, typeof(PerkObject), typeof(bool), typeof(PerkVM.PerkAlternativeType), typeof(Action<PerkVM>), typeof(Action<PerkVM>), typeof(Func<PerkObject, bool>), typeof(Func<PerkObject, bool>))]
internal static class PerkVMCtorPatch
{
    private static void Postfix(PerkVM __instance)
    {
        if (__instance == null) return;
        MasteryTooltip.Install(__instance);
    }
}

// SMR-FEATURE: Mastery perk becomes selectable / auto-assigned in the character screen | gate: IsEligible | Postfix: vanilla yields EarnedAndNotActive for the sibling of an owned perk; override that state
[HarmonyPatch(typeof(PerkVM), nameof(PerkVM.RefreshState))]
internal static class PerkVMRefreshStatePatch
{
    // Fragile: private vanilla name, re-verify in dnSpy on game updates.
    private static readonly FieldInfo? CurrentStateField = AccessTools.Field(typeof(PerkVM), "_currentState");

    private static void Postfix(PerkVM __instance)
    {
        if (__instance == null) return;
        MasteryTooltip.Install(__instance);
        if (__instance.CurrentState != PerkVM.PerkStates.EarnedAndNotActive) return;
        PerkObject? perk = __instance.Perk;
        if (perk?.AlternativePerk == null) return;
        Hero? hero = PerkVMContext.GetHero(__instance);
        if (hero == null || !MasteryRules.IsEligible(hero, perk)) return;

        if (MasteryRules.IsAutoAssign(hero))
        {
            // Re-entry is safe: the perk is owned now, vanilla derives EarnedAndActive and IsCandidate fails.
            if (MasteryGrant.TryGrant(hero, perk)) __instance.RefreshState();
            return;
        }

        if (CurrentStateField == null)
        {
            SmrLog.WarnOnce("PerkVM._currentState", "PerkVM._currentState not found; mastery perks cannot be marked selectable.");
            return;
        }
        CurrentStateField.SetValue(__instance, PerkVM.PerkStates.EarnedButNotSelected);
        __instance.PerkState = (int)PerkVM.PerkStates.EarnedButNotSelected; // public setter raises the property change
    }
}

// SMR-FEATURE: Click unlocks an eligible mastery perk | gate: IsEligible | Prefix: vanilla would open the pair-selection popup; grant directly and skip it
[HarmonyPatch(typeof(PerkVM), nameof(PerkVM.ExecuteStartSelection))]
internal static class PerkVMExecuteStartSelectionPatch
{
    private static bool Prefix(PerkVM __instance)
    {
        if (__instance == null) return true;
        PerkObject? perk = __instance.Perk;
        if (perk?.AlternativePerk == null) return true;
        Hero? hero = PerkVMContext.GetHero(__instance);
        if (hero == null || !MasteryRules.IsEligible(hero, perk)) return true;

        if (MasteryGrant.TryGrant(hero, perk))
        {
            MasteryMessages.OnMastered(hero, perk);
            __instance.RefreshState();
            PerkVMContext.RefreshUnopenedPerks(__instance);
        }
        return false;
    }
}