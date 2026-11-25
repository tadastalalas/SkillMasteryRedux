using System;
using SkillMasteryRedux.Core;

namespace SkillMasteryRedux.Data;

// Shape of ModConfiguration.xml and the only place where defaults and ranges live (MCMSettings uses them too).
public class ModConfiguration
{
    public const bool ShowMessagesDefault = true;
    public const bool DisableSafeguardsDefault = false;
    public const bool ShowSkillLimitDefault = true;
    public const bool ShowHeroListMarkersDefault = true;
    public const int MasteryLevelOffsetDefault = 20;
    public const int MasteryLevelOffsetMin = 1;
    public const int MasteryLevelOffsetMax = 50;
    public const int MasteryStartingLevelOffsetDefault = 0;
    public const int MasteryStartingLevelOffsetMin = -100;
    public const int MasteryStartingLevelOffsetMax = 0;
    public const bool AutoAssignPlayerDefault = false;
    public const bool AllowCompanionsDefault = true;
    public const bool AutoAssignCompanionsDefault = true;
    public const bool AllowAILordsDefault = true;
    public const float MinimumLearningRateDefault = 0.3f;
    public const float MinimumLearningRateMin = 0.05f;
    public const float MinimumLearningRateMax = 3f;

    // Attribute bonuses: per-point values are fractions (0.015 = 1.5% per attribute point).
    public const bool EnableAttributeBonusesDefault = true;
    public const bool OverwriteBetterAttributesDefault = true;
    public const bool AttributeGroupEnabledDefault = true;
    public const BonusRecipients BonusRecipientsDefault = BonusRecipients.AllHeroes;
    public const float BonusPerPointMin = 0f;
    public const float BonusPerPointMax = 0.1f;
    public const bool MeleeDamageDefault = true;
    public const float MeleeDamagePerPointDefault = 0.015f;
    public const bool DrawSpeedDefault = true;
    public const float DrawSpeedPerPointDefault = 0.01f;
    // Off: Realistic Battle Mod has its own crush through and slice through rules.
    public const bool CrushThroughDefault = false;
    public const float CrushThroughPerPointDefault = 0.01f;
    public const bool SliceThroughDefault = false;
    public const float SliceThroughPerPointDefault = 0.01f;
    public const bool RangedDamageDefault = true;
    public const float RangedDamagePerPointDefault = 0.015f;
    public const bool AccuracyDefault = true;
    public const float AccuracyPerPointDefault = 0.02f;
    public const bool ReloadSpeedDefault = true;
    public const float ReloadSpeedPerPointDefault = 0.015f;
    public const bool WeaponHandlingDefault = true;
    public const float WeaponHandlingPerPointDefault = 0.01f;
    public const bool MaxHitPointsDefault = true;
    public const float MaxHitPointsPerPointDefault = 0.02f;
    public const bool StaggerResistanceDefault = true;
    public const float StaggerResistancePerPointDefault = 0.02f;
    public const bool AimStabilityDefault = true;
    public const float AimStabilityPerPointDefault = 0.02f;
    public const bool MovementSpeedDefault = true;
    public const float MovementSpeedPerPointDefault = 0.005f;
    public const bool SmithingStaminaDefault = true;
    public const int SmithingStaminaPerPointDefault = 5;
    public const int SmithingStaminaPerPointMin = 0;
    public const int SmithingStaminaPerPointMax = 20;
    public const bool SimulationAdvantageDefault = true;
    public const float SimulationAdvantagePerPointDefault = 0.01f;
    public const bool VictoryRenownDefault = true;
    public const float VictoryRenownPerPointDefault = 0.02f;
    public const bool VictoryMoraleDefault = true;
    public const float VictoryMoralePerPointDefault = 0.02f;
    public const bool PersuasionChanceDefault = true;
    public const float PersuasionChancePerPointDefault = 0.015f;
    public const bool PartyMoraleDefault = true;
    public const float PartyMoralePerPointDefault = 0.01f;
    public const bool PartySizeDefault = true;
    public const float PartySizePerPointDefault = 0.01f;
    public const bool PartyWagesDefault = true;
    public const float PartyWagesPerPointDefault = 0.01f;
    public const bool CompanionLimitDefault = true;
    public const int CompanionLimitPointsPerCompanionDefault = 4;
    public const int CompanionLimitPointsPerCompanionMin = 1;
    public const int CompanionLimitPointsPerCompanionMax = 10;
    public const bool ClanIncomeDefault = true;
    public const float ClanIncomePerPointDefault = 0.01f;
    public const bool VictoryInfluenceDefault = true;
    public const float VictoryInfluencePerPointDefault = 0.02f;
    public const bool ExperienceGainDefault = true;
    public const float ExperienceGainPerPointDefault = 0.01f;
    public const bool RoleExperienceDefault = true;
    public const float RoleExperiencePerPointDefault = 0.02f;
    public const bool HeroHealingDefault = false;
    public const float HeroHealingPerPointDefault = 0.03f;
    public const bool ShowOnlyBonusesInAttributeTooltipDefault = false;

    public const bool LoggingEnabledDefault = false;

    public GlobalSettingsSection GlobalSettings { get; set; } = new();

    public PlayerSettingsSection PlayerSettings { get; set; } = new();

    public CompanionSettingsSection CompanionSettings { get; set; } = new();

    public AILordSettingsSection AILordSettings { get; set; } = new();

    public LearningSettingsSection LearningSettings { get; set; } = new();

    public AttributeBonusSettingsSection AttributeBonusSettings { get; set; } = new();

    // Always the last section.
    public TechnicalSettingsSection TechnicalSettings { get; set; } = new();

    internal void Clamp()
    {
        PlayerSettings.PlayerMasteryLevelOffset = Clamp(PlayerSettings.PlayerMasteryLevelOffset, MasteryLevelOffsetMin, MasteryLevelOffsetMax);
        PlayerSettings.PlayerMasteryStartingLevelOffset = Clamp(PlayerSettings.PlayerMasteryStartingLevelOffset, MasteryStartingLevelOffsetMin, MasteryStartingLevelOffsetMax);
        CompanionSettings.CompanionMasteryLevelOffset = Clamp(CompanionSettings.CompanionMasteryLevelOffset, MasteryLevelOffsetMin, MasteryLevelOffsetMax);
        CompanionSettings.CompanionMasteryStartingLevelOffset = Clamp(CompanionSettings.CompanionMasteryStartingLevelOffset, MasteryStartingLevelOffsetMin, MasteryStartingLevelOffsetMax);

        float rate = LearningSettings.MinimumLearningRate;
        LearningSettings.MinimumLearningRate = float.IsNaN(rate) ? MinimumLearningRateDefault : Math.Min(Math.Max(rate, MinimumLearningRateMin), MinimumLearningRateMax);

        AttributeBonusSettings.Clamp();
    }

    private static int Clamp(int value, int min, int max) => Math.Min(Math.Max(value, min), max);

    private static float ClampPerPoint(float value, float fallback) =>
        float.IsNaN(value) ? fallback : Math.Min(Math.Max(value, BonusPerPointMin), BonusPerPointMax);

    public class GlobalSettingsSection
    {
        public bool ShowMessages { get; set; } = ShowMessagesDefault;
        public bool DisableSafeguards { get; set; } = DisableSafeguardsDefault;
        public bool ShowSkillLimit { get; set; } = ShowSkillLimitDefault;
        public bool ShowHeroListMarkers { get; set; } = ShowHeroListMarkersDefault;
    }

    public class PlayerSettingsSection
    {
        public int PlayerMasteryLevelOffset { get; set; } = MasteryLevelOffsetDefault;
        public int PlayerMasteryStartingLevelOffset { get; set; } = MasteryStartingLevelOffsetDefault;
        public bool AutoAssignPlayer { get; set; } = AutoAssignPlayerDefault;
    }

    public class CompanionSettingsSection
    {
        public bool AllowCompanions { get; set; } = AllowCompanionsDefault;
        public int CompanionMasteryLevelOffset { get; set; } = MasteryLevelOffsetDefault;
        public int CompanionMasteryStartingLevelOffset { get; set; } = MasteryStartingLevelOffsetDefault;
        public bool AutoAssignCompanions { get; set; } = AutoAssignCompanionsDefault;
    }

    public class AILordSettingsSection
    {
        public bool AllowAILords { get; set; } = AllowAILordsDefault;
    }

    public class LearningSettingsSection
    {
        public float MinimumLearningRate { get; set; } = MinimumLearningRateDefault;
    }

    public class AttributeBonusSettingsSection
    {
        public bool EnableAttributeBonuses { get; set; } = EnableAttributeBonusesDefault;
        public bool OverwriteBetterAttributes { get; set; } = OverwriteBetterAttributesDefault;
        public VigorBonusesSection VigorBonuses { get; set; } = new();
        public ControlBonusesSection ControlBonuses { get; set; } = new();
        public EnduranceBonusesSection EnduranceBonuses { get; set; } = new();
        public CunningBonusesSection CunningBonuses { get; set; } = new();
        public SocialBonusesSection SocialBonuses { get; set; } = new();
        public IntelligenceBonusesSection IntelligenceBonuses { get; set; } = new();
        public CharacterScreenSection CharacterScreen { get; set; } = new();

        internal BonusSetting GetBonus(AttributeBonus bonus)
        {
            VigorBonusesSection v = VigorBonuses;
            ControlBonusesSection c = ControlBonuses;
            EnduranceBonusesSection e = EnduranceBonuses;
            CunningBonusesSection n = CunningBonuses;
            SocialBonusesSection s = SocialBonuses;
            IntelligenceBonusesSection i = IntelligenceBonuses;
            return bonus switch
            {
                AttributeBonus.MeleeDamage => new(v.Enabled && v.MeleeDamage, v.MeleeDamagePerPoint, v.MeleeDamageRecipients),
                AttributeBonus.DrawSpeed => new(v.Enabled && v.DrawSpeed, v.DrawSpeedPerPoint, v.DrawSpeedRecipients),
                AttributeBonus.CrushThrough => new(v.Enabled && v.CrushThrough, v.CrushThroughPerPoint, v.CrushThroughRecipients),
                AttributeBonus.SliceThrough => new(v.Enabled && v.SliceThrough, v.SliceThroughPerPoint, v.SliceThroughRecipients),
                AttributeBonus.RangedDamage => new(c.Enabled && c.RangedDamage, c.RangedDamagePerPoint, c.RangedDamageRecipients),
                AttributeBonus.Accuracy => new(c.Enabled && c.Accuracy, c.AccuracyPerPoint, c.AccuracyRecipients),
                AttributeBonus.ReloadSpeed => new(c.Enabled && c.ReloadSpeed, c.ReloadSpeedPerPoint, c.ReloadSpeedRecipients),
                AttributeBonus.WeaponHandling => new(c.Enabled && c.WeaponHandling, c.WeaponHandlingPerPoint, c.WeaponHandlingRecipients),
                AttributeBonus.MaxHitPoints => new(e.Enabled && e.MaxHitPoints, e.MaxHitPointsPerPoint, e.MaxHitPointsRecipients),
                AttributeBonus.StaggerResistance => new(e.Enabled && e.StaggerResistance, e.StaggerResistancePerPoint, e.StaggerResistanceRecipients),
                AttributeBonus.AimStability => new(e.Enabled && e.AimStability, e.AimStabilityPerPoint, e.AimStabilityRecipients),
                AttributeBonus.MovementSpeed => new(e.Enabled && e.MovementSpeed, e.MovementSpeedPerPoint, e.MovementSpeedRecipients),
                AttributeBonus.SmithingStamina => new(e.Enabled && e.SmithingStamina, e.SmithingStaminaPerPoint, e.SmithingStaminaRecipients),
                AttributeBonus.SimulationAdvantage => new(n.Enabled && n.SimulationAdvantage, n.SimulationAdvantagePerPoint, n.SimulationAdvantageRecipients),
                AttributeBonus.VictoryRenown => new(n.Enabled && n.VictoryRenown, n.VictoryRenownPerPoint, n.VictoryRenownRecipients),
                AttributeBonus.VictoryMorale => new(n.Enabled && n.VictoryMorale, n.VictoryMoralePerPoint, n.VictoryMoraleRecipients),
                // Only the player persuades.
                AttributeBonus.PersuasionChance => new(s.Enabled && s.PersuasionChance, s.PersuasionChancePerPoint, BonusRecipients.Player),
                AttributeBonus.PartyMorale => new(s.Enabled && s.PartyMorale, s.PartyMoralePerPoint, s.PartyMoraleRecipients),
                AttributeBonus.PartySize => new(s.Enabled && s.PartySize, s.PartySizePerPoint, s.PartySizeRecipients),
                AttributeBonus.PartyWages => new(s.Enabled && s.PartyWages, s.PartyWagesPerPoint, s.PartyWagesRecipients),
                AttributeBonus.CompanionLimit => new(s.Enabled && s.CompanionLimit, s.CompanionLimitPointsPerCompanion, s.CompanionLimitRecipients),
                AttributeBonus.ClanIncome => new(i.Enabled && i.ClanIncome, i.ClanIncomePerPoint, i.ClanIncomeRecipients),
                AttributeBonus.VictoryInfluence => new(i.Enabled && i.VictoryInfluence, i.VictoryInfluencePerPoint, i.VictoryInfluenceRecipients),
                AttributeBonus.ExperienceGain => new(i.Enabled && i.ExperienceGain, i.ExperienceGainPerPoint, i.ExperienceGainRecipients),
                AttributeBonus.RoleExperience => new(i.Enabled && i.RoleExperience, i.RoleExperiencePerPoint, i.RoleExperienceRecipients),
                AttributeBonus.HeroHealing => new(i.Enabled && i.HeroHealing, i.HeroHealingPerPoint, i.HeroHealingRecipients),
                _ => default,
            };
        }

        internal void Clamp()
        {
            VigorBonuses.MeleeDamagePerPoint = ClampPerPoint(VigorBonuses.MeleeDamagePerPoint, MeleeDamagePerPointDefault);
            VigorBonuses.DrawSpeedPerPoint = ClampPerPoint(VigorBonuses.DrawSpeedPerPoint, DrawSpeedPerPointDefault);
            VigorBonuses.CrushThroughPerPoint = ClampPerPoint(VigorBonuses.CrushThroughPerPoint, CrushThroughPerPointDefault);
            VigorBonuses.SliceThroughPerPoint = ClampPerPoint(VigorBonuses.SliceThroughPerPoint, SliceThroughPerPointDefault);
            ControlBonuses.RangedDamagePerPoint = ClampPerPoint(ControlBonuses.RangedDamagePerPoint, RangedDamagePerPointDefault);
            ControlBonuses.AccuracyPerPoint = ClampPerPoint(ControlBonuses.AccuracyPerPoint, AccuracyPerPointDefault);
            ControlBonuses.ReloadSpeedPerPoint = ClampPerPoint(ControlBonuses.ReloadSpeedPerPoint, ReloadSpeedPerPointDefault);
            ControlBonuses.WeaponHandlingPerPoint = ClampPerPoint(ControlBonuses.WeaponHandlingPerPoint, WeaponHandlingPerPointDefault);
            EnduranceBonuses.MaxHitPointsPerPoint = ClampPerPoint(EnduranceBonuses.MaxHitPointsPerPoint, MaxHitPointsPerPointDefault);
            EnduranceBonuses.StaggerResistancePerPoint = ClampPerPoint(EnduranceBonuses.StaggerResistancePerPoint, StaggerResistancePerPointDefault);
            EnduranceBonuses.AimStabilityPerPoint = ClampPerPoint(EnduranceBonuses.AimStabilityPerPoint, AimStabilityPerPointDefault);
            EnduranceBonuses.MovementSpeedPerPoint = ClampPerPoint(EnduranceBonuses.MovementSpeedPerPoint, MovementSpeedPerPointDefault);
            EnduranceBonuses.SmithingStaminaPerPoint = ModConfiguration.Clamp(EnduranceBonuses.SmithingStaminaPerPoint, SmithingStaminaPerPointMin, SmithingStaminaPerPointMax);
            CunningBonuses.SimulationAdvantagePerPoint = ClampPerPoint(CunningBonuses.SimulationAdvantagePerPoint, SimulationAdvantagePerPointDefault);
            CunningBonuses.VictoryRenownPerPoint = ClampPerPoint(CunningBonuses.VictoryRenownPerPoint, VictoryRenownPerPointDefault);
            CunningBonuses.VictoryMoralePerPoint = ClampPerPoint(CunningBonuses.VictoryMoralePerPoint, VictoryMoralePerPointDefault);
            SocialBonuses.PersuasionChancePerPoint = ClampPerPoint(SocialBonuses.PersuasionChancePerPoint, PersuasionChancePerPointDefault);
            SocialBonuses.PartyMoralePerPoint = ClampPerPoint(SocialBonuses.PartyMoralePerPoint, PartyMoralePerPointDefault);
            SocialBonuses.PartySizePerPoint = ClampPerPoint(SocialBonuses.PartySizePerPoint, PartySizePerPointDefault);
            SocialBonuses.PartyWagesPerPoint = ClampPerPoint(SocialBonuses.PartyWagesPerPoint, PartyWagesPerPointDefault);
            SocialBonuses.CompanionLimitPointsPerCompanion = ModConfiguration.Clamp(SocialBonuses.CompanionLimitPointsPerCompanion, CompanionLimitPointsPerCompanionMin, CompanionLimitPointsPerCompanionMax);
            IntelligenceBonuses.ClanIncomePerPoint = ClampPerPoint(IntelligenceBonuses.ClanIncomePerPoint, ClanIncomePerPointDefault);
            IntelligenceBonuses.VictoryInfluencePerPoint = ClampPerPoint(IntelligenceBonuses.VictoryInfluencePerPoint, VictoryInfluencePerPointDefault);
            IntelligenceBonuses.ExperienceGainPerPoint = ClampPerPoint(IntelligenceBonuses.ExperienceGainPerPoint, ExperienceGainPerPointDefault);
            IntelligenceBonuses.RoleExperiencePerPoint = ClampPerPoint(IntelligenceBonuses.RoleExperiencePerPoint, RoleExperiencePerPointDefault);
            IntelligenceBonuses.HeroHealingPerPoint = ClampPerPoint(IntelligenceBonuses.HeroHealingPerPoint, HeroHealingPerPointDefault);
        }
    }

    public class VigorBonusesSection
    {
        public bool Enabled { get; set; } = AttributeGroupEnabledDefault;
        public bool MeleeDamage { get; set; } = MeleeDamageDefault;
        public float MeleeDamagePerPoint { get; set; } = MeleeDamagePerPointDefault;
        public BonusRecipients MeleeDamageRecipients { get; set; } = BonusRecipientsDefault;
        public bool DrawSpeed { get; set; } = DrawSpeedDefault;
        public float DrawSpeedPerPoint { get; set; } = DrawSpeedPerPointDefault;
        public BonusRecipients DrawSpeedRecipients { get; set; } = BonusRecipientsDefault;
        public bool CrushThrough { get; set; } = CrushThroughDefault;
        public float CrushThroughPerPoint { get; set; } = CrushThroughPerPointDefault;
        public BonusRecipients CrushThroughRecipients { get; set; } = BonusRecipientsDefault;
        public bool SliceThrough { get; set; } = SliceThroughDefault;
        public float SliceThroughPerPoint { get; set; } = SliceThroughPerPointDefault;
        public BonusRecipients SliceThroughRecipients { get; set; } = BonusRecipientsDefault;
    }

    public class ControlBonusesSection
    {
        public bool Enabled { get; set; } = AttributeGroupEnabledDefault;
        public bool RangedDamage { get; set; } = RangedDamageDefault;
        public float RangedDamagePerPoint { get; set; } = RangedDamagePerPointDefault;
        public BonusRecipients RangedDamageRecipients { get; set; } = BonusRecipientsDefault;
        public bool Accuracy { get; set; } = AccuracyDefault;
        public float AccuracyPerPoint { get; set; } = AccuracyPerPointDefault;
        public BonusRecipients AccuracyRecipients { get; set; } = BonusRecipientsDefault;
        public bool ReloadSpeed { get; set; } = ReloadSpeedDefault;
        public float ReloadSpeedPerPoint { get; set; } = ReloadSpeedPerPointDefault;
        public BonusRecipients ReloadSpeedRecipients { get; set; } = BonusRecipientsDefault;
        public bool WeaponHandling { get; set; } = WeaponHandlingDefault;
        public float WeaponHandlingPerPoint { get; set; } = WeaponHandlingPerPointDefault;
        public BonusRecipients WeaponHandlingRecipients { get; set; } = BonusRecipientsDefault;
    }

    public class EnduranceBonusesSection
    {
        public bool Enabled { get; set; } = AttributeGroupEnabledDefault;
        public bool MaxHitPoints { get; set; } = MaxHitPointsDefault;
        public float MaxHitPointsPerPoint { get; set; } = MaxHitPointsPerPointDefault;
        public BonusRecipients MaxHitPointsRecipients { get; set; } = BonusRecipientsDefault;
        public bool StaggerResistance { get; set; } = StaggerResistanceDefault;
        public float StaggerResistancePerPoint { get; set; } = StaggerResistancePerPointDefault;
        public BonusRecipients StaggerResistanceRecipients { get; set; } = BonusRecipientsDefault;
        public bool AimStability { get; set; } = AimStabilityDefault;
        public float AimStabilityPerPoint { get; set; } = AimStabilityPerPointDefault;
        public BonusRecipients AimStabilityRecipients { get; set; } = BonusRecipientsDefault;
        public bool MovementSpeed { get; set; } = MovementSpeedDefault;
        public float MovementSpeedPerPoint { get; set; } = MovementSpeedPerPointDefault;
        public BonusRecipients MovementSpeedRecipients { get; set; } = BonusRecipientsDefault;
        public bool SmithingStamina { get; set; } = SmithingStaminaDefault;
        public int SmithingStaminaPerPoint { get; set; } = SmithingStaminaPerPointDefault;
        public BonusRecipients SmithingStaminaRecipients { get; set; } = BonusRecipientsDefault;
    }

    public class CunningBonusesSection
    {
        public bool Enabled { get; set; } = AttributeGroupEnabledDefault;
        public bool SimulationAdvantage { get; set; } = SimulationAdvantageDefault;
        public float SimulationAdvantagePerPoint { get; set; } = SimulationAdvantagePerPointDefault;
        public BonusRecipients SimulationAdvantageRecipients { get; set; } = BonusRecipientsDefault;
        public bool VictoryRenown { get; set; } = VictoryRenownDefault;
        public float VictoryRenownPerPoint { get; set; } = VictoryRenownPerPointDefault;
        public BonusRecipients VictoryRenownRecipients { get; set; } = BonusRecipientsDefault;
        public bool VictoryMorale { get; set; } = VictoryMoraleDefault;
        public float VictoryMoralePerPoint { get; set; } = VictoryMoralePerPointDefault;
        public BonusRecipients VictoryMoraleRecipients { get; set; } = BonusRecipientsDefault;
    }

    public class SocialBonusesSection
    {
        public bool Enabled { get; set; } = AttributeGroupEnabledDefault;
        public bool PersuasionChance { get; set; } = PersuasionChanceDefault;
        public float PersuasionChancePerPoint { get; set; } = PersuasionChancePerPointDefault;
        public bool PartyMorale { get; set; } = PartyMoraleDefault;
        public float PartyMoralePerPoint { get; set; } = PartyMoralePerPointDefault;
        public BonusRecipients PartyMoraleRecipients { get; set; } = BonusRecipientsDefault;
        public bool PartySize { get; set; } = PartySizeDefault;
        public float PartySizePerPoint { get; set; } = PartySizePerPointDefault;
        public BonusRecipients PartySizeRecipients { get; set; } = BonusRecipientsDefault;
        public bool PartyWages { get; set; } = PartyWagesDefault;
        public float PartyWagesPerPoint { get; set; } = PartyWagesPerPointDefault;
        public BonusRecipients PartyWagesRecipients { get; set; } = BonusRecipientsDefault;
        public bool CompanionLimit { get; set; } = CompanionLimitDefault;
        public int CompanionLimitPointsPerCompanion { get; set; } = CompanionLimitPointsPerCompanionDefault;
        public BonusRecipients CompanionLimitRecipients { get; set; } = BonusRecipientsDefault;
    }

    public class IntelligenceBonusesSection
    {
        public bool Enabled { get; set; } = AttributeGroupEnabledDefault;
        public bool ClanIncome { get; set; } = ClanIncomeDefault;
        public float ClanIncomePerPoint { get; set; } = ClanIncomePerPointDefault;
        public BonusRecipients ClanIncomeRecipients { get; set; } = BonusRecipientsDefault;
        public bool VictoryInfluence { get; set; } = VictoryInfluenceDefault;
        public float VictoryInfluencePerPoint { get; set; } = VictoryInfluencePerPointDefault;
        public BonusRecipients VictoryInfluenceRecipients { get; set; } = BonusRecipientsDefault;
        public bool ExperienceGain { get; set; } = ExperienceGainDefault;
        public float ExperienceGainPerPoint { get; set; } = ExperienceGainPerPointDefault;
        public BonusRecipients ExperienceGainRecipients { get; set; } = BonusRecipientsDefault;
        public bool RoleExperience { get; set; } = RoleExperienceDefault;
        public float RoleExperiencePerPoint { get; set; } = RoleExperiencePerPointDefault;
        public BonusRecipients RoleExperienceRecipients { get; set; } = BonusRecipientsDefault;
        public bool HeroHealing { get; set; } = HeroHealingDefault;
        public float HeroHealingPerPoint { get; set; } = HeroHealingPerPointDefault;
        public BonusRecipients HeroHealingRecipients { get; set; } = BonusRecipientsDefault;
    }

    public class CharacterScreenSection
    {
        public bool ShowOnlyBonusesInAttributeTooltip { get; set; } = ShowOnlyBonusesInAttributeTooltipDefault;
    }

    public class TechnicalSettingsSection
    {
        // Always the last entry.
        public bool LoggingEnabled { get; set; } = LoggingEnabledDefault;
    }
}
