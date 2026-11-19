using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SkillMasteryRedux.Core;

// Read-only rules facade: who may master, which perk is a mastery candidate, at what level.
internal static class MasteryRules
{
    // Per skill: level of its highest perk of any kind (vanilla's unpaired ultimate perk) and the tier index of its highest paired perk.
    private static readonly Dictionary<SkillObject, (int Top, int PairedIndex)> Tiers = new();

    internal static bool ShowMessages => ModSettings.ShowMessages;

    internal static bool ShowSkillLimit => ModSettings.ShowSkillLimit;

    internal static bool ShowHeroListMarkers => ModSettings.ShowHeroListMarkers;

    internal static float MinimumLearningRate => ModSettings.MinimumLearningRate;

    internal static bool IsPlayer(Hero hero) => hero == Hero.MainHero;

    internal static bool IsPlayerClan(Hero hero) => hero.Clan != null && hero.Clan == Clan.PlayerClan;

    // Player always; clan heroes via AllowCompanions; other lords via AllowAILords.
    internal static bool IsEnabledFor(Hero hero)
    {
        if (IsPlayer(hero)) return true;
        if (IsPlayerClan(hero)) return ModSettings.AllowCompanions;
        return ModSettings.AllowAILords && hero.IsLord;
    }

    internal static bool IsAutoAssign(Hero hero)
    {
        if (IsPlayer(hero)) return ModSettings.AutoAssignPlayer;
        return !IsPlayerClan(hero) || ModSettings.AutoAssignCompanions;
    }

    // Daily tick targets: every enabled non-player hero whose perks are assigned automatically.
    internal static bool IsAutoMasteryHero(Hero hero) =>
        !IsPlayer(hero) && hero.IsAlive && !hero.IsChild && hero.HeroDeveloper != null && IsEnabledFor(hero) && IsAutoAssign(hero);

    // The sibling of an owned perk that the hero does not own yet.
    internal static bool IsCandidate(Hero hero, PerkObject perk) =>
        perk.AlternativePerk != null && !hero.GetPerkValue(perk) && hero.GetPerkValue(perk.AlternativePerk);

    internal static bool HasRequiredSkill(Hero hero, PerkObject perk) => hero.GetSkillValue(perk.Skill) >= RequiredLevel(perk, hero);

    internal static bool IsEligible(Hero hero, PerkObject perk) => IsEnabledFor(hero) && IsCandidate(hero, perk) && HasRequiredSkill(hero, perk);

    // Mastery perks exist only for paired tiers; the last one is the alternative of the highest paired perk.
    internal static bool IsFinalTier(PerkObject perk) => TierIndex(perk) >= TopPairedIndex(perk.Skill);

    internal static int RequiredLevel(PerkObject perk, Hero hero) => RequiredLevel(perk, IsPlayer(hero));

    // start = level of the skill's top vanilla perk + starting offset; required = start + tierIndex * offset.
    // Safeguard: offset is reduced so the last mastery tier (highest paired tier) lands at or below SkillCap.
    internal static int RequiredLevel(PerkObject perk, bool isPlayer)
    {
        int topPairedIndex = TopPairedIndex(perk.Skill);
        if (topPairedIndex < 1) topPairedIndex = 1;
        int tierIndex = TierIndex(perk);
        int start = TopTier(perk.Skill) + (isPlayer ? ModSettings.PlayerMasteryStartingLevelOffset : ModSettings.CompanionMasteryStartingLevelOffset);
        int offset = isPlayer ? ModSettings.PlayerMasteryLevelOffset : ModSettings.CompanionMasteryLevelOffset;
        if (!ModSettings.DisableSafeguards)
        {
            int maxOffset = (MasteryBalance.SkillCap - start) / topPairedIndex;
            if (maxOffset < 0) maxOffset = 0;
            if (offset > maxOffset) offset = maxOffset;
        }
        return start + tierIndex * offset;
    }

    internal static int TierIndex(PerkObject perk) => (int)(perk.RequiredSkillValue / MasteryBalance.TierStep);

    // Level of the skill's highest perk, paired or not (vanilla: 300 Trade, 250 Two Handed, 275 others; all unpaired ultimates).
    internal static int TopTier(SkillObject skill) => GetTiers(skill).Top;

    // Tier index of the skill's highest paired perk (vanilla: 11 Trade, 9 Two Handed, 10 others).
    internal static int TopPairedIndex(SkillObject skill) => GetTiers(skill).PairedIndex;

    private static (int Top, int PairedIndex) GetTiers(SkillObject skill)
    {
        if (Tiers.TryGetValue(skill, out (int Top, int PairedIndex) cached)) return cached;
        float top = 0f;
        float topPaired = 0f;
        MBReadOnlyList<PerkObject> perks = PerkObject.All;
        for (int i = 0; i < perks.Count; i++)
        {
            PerkObject p = perks[i];
            if (p.Skill != skill) continue;
            if (p.RequiredSkillValue > top) top = p.RequiredSkillValue;
            if (p.AlternativePerk != null && p.RequiredSkillValue > topPaired) topPaired = p.RequiredSkillValue;
        }
        (int Top, int PairedIndex) result = ((int)top, (int)(topPaired / MasteryBalance.TierStep));
        Tiers[skill] = result;
        return result;
    }

    // Perk objects are recreated per game; drop cached references.
    internal static void ResetCache() => Tiers.Clear();
}