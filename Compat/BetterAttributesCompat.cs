using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;

namespace SkillMasteryRedux.Compat;

// SMR-FEATURE: Overwrite Better Attributes Mod | gate: setting:OverwriteBetterAttributes | Prefix: Better Attributes' wage model adds its own bonus after calling base; return the base result so only this mod's bonus applies
// Removes every Harmony patch whose patch method lives in the Better Attributes assembly (one patch at a time, never UnpatchAll).
internal static class BetterAttributesCompat
{
    // Fragile: Better Attributes type names (decompiled 1.4.8 build), re-verify when that mod updates.
    private const string SubModuleTypeName = "BetterAttributes.BetterAttributes";
    private const string WageModelTypeName = "BetterAttributes.Custom.CustomDefaultPartyWageModel";

    private static readonly Type[] TotalWageArguments = [typeof(MobileParty), typeof(TroopRoster), typeof(bool)];

    private static Func<DefaultPartyWageModel, MobileParty, TroopRoster, bool, ExplainedNumber>? _baseTotalWage;
    private static bool _isApplied;

    // Called once all modules have applied their patches.
    internal static void TryApply(Harmony harmony)
    {
        if (_isApplied) return;
        _isApplied = true;

        Type? subModule = AccessTools.TypeByName(SubModuleTypeName);
        if (subModule == null) return;

        // Settings are otherwise read on campaign start; this check runs at the main menu.
        ModSettings.Reload();
        if (!AttributeBonusRules.OverwriteBetterAttributes)
        {
            SmrLog.Warn("Better Attributes detected and \"Overwrite Better Attributes Mod\" is off: both mods give attribute bonuses and they add up.");
            return;
        }

        try
        {
            int removed = RemovePatches(harmony, subModule.Assembly);
            bool isWageModelOff = PatchWageModel(harmony);
            SmrLog.Warn("Better Attributes detected and overwritten: " + removed + " of its patches removed" +
                        (isWageModelOff ? ", its wage model reduced to the base game's." : "; its wage model was not found.") +
                        " Disable it; SkillMasteryRedux includes its bonuses.");
        }
        catch (Exception ex)
        {
            SmrLog.Error("Overwriting Better Attributes failed; both mods may give attribute bonuses.", ex);
        }
    }

    private static int RemovePatches(Harmony harmony, Assembly assembly)
    {
        List<(MethodBase Original, MethodInfo Patch)> found = [];
        foreach (MethodBase original in Harmony.GetAllPatchedMethods())
        {
            // Fully qualified: the SkillMasteryRedux.Patches namespace hides HarmonyLib.Patches here.
            HarmonyLib.Patches? info = Harmony.GetPatchInfo(original);
            if (info == null) continue;
            Collect(found, original, info.Prefixes, assembly);
            Collect(found, original, info.Postfixes, assembly);
            Collect(found, original, info.Transpilers, assembly);
            Collect(found, original, info.Finalizers, assembly);
        }

        for (int i = 0; i < found.Count; i++)
        {
            harmony.Unpatch(found[i].Original, found[i].Patch);
            SmrLog.Debug("Removed Better Attributes patch " + found[i].Patch.Name + " from " + found[i].Original.DeclaringType?.FullName + "." + found[i].Original.Name + ".");
        }

        return found.Count;
    }

    private static void Collect(List<(MethodBase Original, MethodInfo Patch)> found, MethodBase original, IEnumerable<Patch> patches, Assembly assembly)
    {
        foreach (Patch patch in patches)
        {
            if (patch.PatchMethod?.DeclaringType?.Assembly == assembly) found.Add((original, patch.PatchMethod));
        }
    }

    private static bool PatchWageModel(Harmony harmony)
    {
        Type? wageModel = AccessTools.TypeByName(WageModelTypeName);
        MethodInfo? target = wageModel == null ? null : AccessTools.DeclaredMethod(wageModel, nameof(DefaultPartyWageModel.GetTotalWage), TotalWageArguments);
        MethodInfo? baseMethod = AccessTools.DeclaredMethod(typeof(DefaultPartyWageModel), nameof(DefaultPartyWageModel.GetTotalWage), TotalWageArguments);
        if (target == null || baseMethod == null) return false;

        // Non-virtual: calls DefaultPartyWageModel's own GetTotalWage (with this mod's wage postfix) on Better Attributes' model.
        _baseTotalWage = AccessTools.MethodDelegate<Func<DefaultPartyWageModel, MobileParty, TroopRoster, bool, ExplainedNumber>>(baseMethod, null, virtualCall: false, delegateArgs: null);
        harmony.Patch(target, prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(BetterAttributesCompat), nameof(WageModelPrefix))));
        return true;
    }

    private static bool WageModelPrefix(DefaultPartyWageModel __instance, MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions, ref ExplainedNumber __result)
    {
        if (_baseTotalWage == null) return true;
        __result = _baseTotalWage(__instance, mobileParty, troopRoster, includeDescriptions);
        return false;
    }
}
