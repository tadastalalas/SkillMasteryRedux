using System;
using System.Reflection;
using HarmonyLib;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using SkillMasteryRedux.UI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Attribute tooltip lists the attribute's bonuses at its current value | gate: setting:EnableAttributeBonuses | Postfix: the constructor has run vanilla RefreshValues (increase hint) by now; put the bonus list in front of it or in its place
[HarmonyPatch(typeof(CharacterAttributeItemVM), MethodType.Constructor,
    typeof(Hero), typeof(CharacterAttribute), typeof(CharacterDeveloperHeroItemVM), typeof(Action<CharacterAttributeItemVM>), typeof(Action<CharacterAttributeItemVM>))]
internal static class CharacterAttributeItemVMCtorPatch
{
    private static void Postfix(CharacterAttributeItemVM __instance, Hero hero)
    {
        if (__instance == null) return;
        AttributeBonusTooltip.OnRefreshValues(__instance, hero);
        CharacterAttributeItemVMRefreshValuesPatch.TryApply();
    }
}

// SMR-FEATURE: Attribute tooltip lists the attribute's bonuses at its current value | gate: setting:EnableAttributeBonuses | Postfix: RefreshValues called again later rewrites vanilla's increase hint; add the list again
// Not an attribute patch: RefreshValues reads the static field CampaignUIHelper.SkillObjectComparerInstance, and CampaignUIHelper is
// beforefieldinit (1.4.7, 1.4.8, 1.5.7). Patching it at module load compiles that read before any game exists, which can run
// CampaignUIHelper's static initializer too early and break the class for the session (crash on save load in GetDenarTooltip).
// Applied once from the constructor postfix, after vanilla RefreshValues has run.
internal static class CharacterAttributeItemVMRefreshValuesPatch
{
    private static bool _isApplied;

    internal static void TryApply()
    {
        if (_isApplied) return;
        _isApplied = true;
        MethodInfo? target = AccessTools.DeclaredMethod(typeof(CharacterAttributeItemVM), nameof(CharacterAttributeItemVM.RefreshValues), Type.EmptyTypes);
        MethodInfo? postfix = AccessTools.DeclaredMethod(typeof(CharacterAttributeItemVMRefreshValuesPatch), nameof(Postfix));
        if (target == null || postfix == null)
        {
            SmrLog.Warn("CharacterAttributeItemVM.RefreshValues not found; the attribute tooltip keeps the bonus list only until the game refreshes it.");
            return;
        }

        try
        {
            new Harmony(ModInfo.HarmonyId).Patch(target, postfix: new HarmonyMethod(postfix));
        }
        catch (Exception ex)
        {
            SmrLog.Error("Patching CharacterAttributeItemVM.RefreshValues failed.", ex);
        }
    }

    // ____hero: vanilla private field _hero (re-verify in dnSpy on game updates).
    private static void Postfix(CharacterAttributeItemVM __instance, Hero ____hero)
    {
        if (__instance == null) return;
        AttributeBonusTooltip.OnRefreshValues(__instance, ____hero);
    }
}

// SMR-FEATURE: Attribute tooltip lists the attribute's bonuses at its current value | gate: setting:EnableAttributeBonuses | Postfix: RefreshWithCurrentValues updates AttributeValue but not the hint; rebuild the list for the new value
[HarmonyPatch(typeof(CharacterAttributeItemVM), nameof(CharacterAttributeItemVM.RefreshWithCurrentValues), new Type[] { })]
internal static class CharacterAttributeItemVMRefreshWithCurrentValuesPatch
{
    private static void Postfix(CharacterAttributeItemVM __instance, Hero ____hero)
    {
        if (__instance == null) return;
        AttributeBonusTooltip.OnRefreshWithCurrentValues(__instance, ____hero);
    }
}
