using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SkillMasteryRedux.Core;

// Hero dropdown perk count from game data, for heroes whose skill VMs are not built yet (e.g. PerkQOL lazy init).
// Mirrors vanilla SkillVM/PerkVM state rules plus SMR mastery: one per (skill, tier) with a selectable perk.
internal static class UnopenedPerkCounter
{
    private static readonly HashSet<(SkillObject Skill, int Tier)> CountedTiers = [];

    internal static int Count(Hero hero, PerkSelectionVM? pending)
    {
        CountedTiers.Clear();
        bool isAutoAssign = MasteryRules.IsAutoAssign(hero);
        int count = 0;
        MBReadOnlyList<PerkObject> perks = PerkObject.All;
        for (int i = 0; i < perks.Count; i++)
        {
            PerkObject perk = perks[i];
            SkillObject? skill = perk.Skill;
            if (skill == null || perk.RequiredSkillValue > hero.GetSkillValue(skill)) continue;
            if (IsSelected(hero, pending, perk)) continue;

            PerkObject? alternative = perk.AlternativePerk;
            bool isSelectable = alternative != null && IsSelected(hero, pending, alternative)
                ? !isAutoAssign && MasteryRules.IsEligible(hero, perk)
                : IsPreviousSelected(hero, pending, perk);
            if (isSelectable && CountedTiers.Add((skill, (int)perk.RequiredSkillValue))) count++;
        }
        return count;
    }

    private static bool IsSelected(Hero hero, PerkSelectionVM? pending, PerkObject perk) =>
        hero.GetPerkValue(perk) || pending?.IsPerkSelected(perk) == true;

    // Vanilla SkillVM.IsPreviousPerkSelected: the closest lower-tier perk of the skill, or its alternative, is selected.
    private static bool IsPreviousSelected(Hero hero, PerkSelectionVM? pending, PerkObject perk)
    {
        PerkObject? previous = null;
        MBReadOnlyList<PerkObject> perks = PerkObject.All;
        for (int i = 0; i < perks.Count; i++)
        {
            PerkObject p = perks[i];
            if (p.Skill != perk.Skill || p.RequiredSkillValue >= perk.RequiredSkillValue) continue;
            if (previous == null || p.RequiredSkillValue > previous.RequiredSkillValue) previous = p;
        }
        if (previous == null) return true;
        return IsSelected(hero, pending, previous) || (previous.AlternativePerk != null && IsSelected(hero, pending, previous.AlternativePerk));
    }
}
