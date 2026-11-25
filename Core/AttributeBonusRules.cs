using System;
using SkillMasteryRedux.Data;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SkillMasteryRedux.Core;

// Read-only rules facade for attribute bonuses: which attribute drives a bonus, who receives it and how big it is.
internal static class AttributeBonusRules
{
    internal const int BonusCount = (int)AttributeBonus.HeroHealing + 1;

    internal static bool IsEnabled => ModSettings.AttributeBonusesEnabled;

    internal static bool OverwriteBetterAttributes => ModSettings.OverwriteBetterAttributes;

    internal static bool ShowOnlyBonusesInAttributeTooltip => ModSettings.ShowOnlyBonusesInAttributeTooltip;

    // Bonus from the hero's own attribute. 0 when the bonus is off, the hero is not a recipient or has no points.
    internal static float Value(AttributeBonus bonus, Hero? hero)
    {
        if (hero == null || !ModSettings.AttributeBonusesEnabled) return 0f;
        BonusSetting setting = ModSettings.Bonus(bonus);
        if (!setting.IsEnabled || !IsRecipient(hero, setting.Recipients)) return 0f;
        return Scale(bonus, setting, hero.GetAttributeValue(AttributeOf(bonus)));
    }

    // Same with a given attribute value: the character screen shows the value it is about to confirm.
    internal static float Value(AttributeBonus bonus, Hero hero, int points)
    {
        if (!ModSettings.AttributeBonusesEnabled) return 0f;
        BonusSetting setting = ModSettings.Bonus(bonus);
        if (!setting.IsEnabled || !IsRecipient(hero, setting.Recipients)) return 0f;
        return Scale(bonus, setting, points);
    }

    // Adds the bonus as a factor labelled with the attribute name, so vanilla's breakdown tooltips show where it comes from.
    internal static void AddFactor(ref ExplainedNumber number, AttributeBonus bonus, Hero? hero, bool isReduction = false)
    {
        float value = Value(bonus, hero);
        if (value > 0f) number.AddFactor(isReduction ? -value : value, AttributeOf(bonus).Name);
    }

    internal static bool IsRecipient(Hero hero, BonusRecipients recipients) => recipients switch
    {
        BonusRecipients.Player => MasteryRules.IsPlayer(hero),
        BonusRecipients.PlayerClan => MasteryRules.IsPlayerClan(hero),
        _ => true,
    };

    // Heroes only: troops have no attributes. CharacterObject exists only in the campaign.
    internal static Hero? HeroOf(BasicCharacterObject? character) =>
        Campaign.Current != null && character is CharacterObject { IsHero: true } hero ? hero.HeroObject : null;

    internal static Hero? HeroOf(Agent? agent) => agent == null ? null : HeroOf(agent.Character);

    internal static CharacterAttribute AttributeOf(AttributeBonus bonus) => bonus switch
    {
        AttributeBonus.MeleeDamage or AttributeBonus.DrawSpeed or AttributeBonus.CrushThrough or AttributeBonus.SliceThrough
            => DefaultCharacterAttributes.Vigor,
        AttributeBonus.RangedDamage or AttributeBonus.Accuracy or AttributeBonus.ReloadSpeed or AttributeBonus.WeaponHandling
            => DefaultCharacterAttributes.Control,
        AttributeBonus.MaxHitPoints or AttributeBonus.StaggerResistance or AttributeBonus.AimStability or AttributeBonus.MovementSpeed or AttributeBonus.SmithingStamina
            => DefaultCharacterAttributes.Endurance,
        AttributeBonus.SimulationAdvantage or AttributeBonus.VictoryRenown or AttributeBonus.VictoryMorale
            => DefaultCharacterAttributes.Cunning,
        AttributeBonus.PersuasionChance or AttributeBonus.PartyMorale or AttributeBonus.PartySize or AttributeBonus.PartyWages or AttributeBonus.CompanionLimit
            => DefaultCharacterAttributes.Social,
        AttributeBonus.ClanIncome or AttributeBonus.VictoryInfluence or AttributeBonus.ExperienceGain or AttributeBonus.RoleExperience or AttributeBonus.HeroHealing
            => DefaultCharacterAttributes.Intelligence,
        _ => throw new ArgumentOutOfRangeException(nameof(bonus), bonus, null),
    };

    // CompanionLimit: one extra companion per Amount points. Everything else: Amount per point.
    private static float Scale(AttributeBonus bonus, BonusSetting setting, int points)
    {
        if (points <= 0) return 0f;
        if (bonus != AttributeBonus.CompanionLimit) return setting.Amount * points;
        int pointsPerCompanion = (int)setting.Amount;
        return pointsPerCompanion < 1 ? 0f : points / pointsPerCompanion;
    }
}
