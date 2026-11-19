using System.Collections.Generic;
using System.Collections.ObjectModel;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Unopened-perk badge counts selectable mastery perks | gate: always | Postfix: count one per tier (a mastery tier has a single selectable perk) and write through the notifying setter
[HarmonyPatch(typeof(SkillVM), "RefreshNumOfUnopenedPerks")]
internal static class SkillVMPatch
{
    private static readonly HashSet<int> CountedTiers = [];

    private static void Postfix(SkillVM __instance)
    {
        if (__instance == null) return;
        Collection<PerkVM>? perks = __instance.Perks;
        if (perks == null) return;
        CountedTiers.Clear();
        int count = 0;
        for (int i = 0; i < perks.Count; i++)
        {
            PerkVM perk = perks[i];
            if (perk?.Perk == null || perk.CurrentState != PerkVM.PerkStates.EarnedButNotSelected) continue;
            if (CountedTiers.Add((int)perk.Perk.RequiredSkillValue)) count++;
        }
        __instance.NumOfUnopenedPerks = count;
    }
}