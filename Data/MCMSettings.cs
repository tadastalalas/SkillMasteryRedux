using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;
using SkillMasteryRedux.Core;

namespace SkillMasteryRedux.Data;

// Used only when MCM is loaded. The only file that may use MCM types.
internal sealed class MCMSettings : AttributeGlobalSettings<MCMSettings>
{
    // Released Id, folder and format: changing them resets every player's saved settings.
    private const string SettingsId = "SkillMasteryRedux";
    private const string SaveFormat = "json";
    private const string WholeNumberFormat = "0";
    private const string TwoDecimalsFormat = "0.00";
    private const string PercentFormat = "0.0%";

    private const string GroupAttributeBonuses = Texts.Ids.GroupAttributeBonuses;
    private const string GroupVigor = GroupAttributeBonuses + "/" + Texts.Ids.GroupVigor;
    private const string GroupControl = GroupAttributeBonuses + "/" + Texts.Ids.GroupControl;
    private const string GroupEndurance = GroupAttributeBonuses + "/" + Texts.Ids.GroupEndurance;
    private const string GroupCunning = GroupAttributeBonuses + "/" + Texts.Ids.GroupCunning;
    private const string GroupSocial = GroupAttributeBonuses + "/" + Texts.Ids.GroupSocial;
    private const string GroupIntelligence = GroupAttributeBonuses + "/" + Texts.Ids.GroupIntelligence;
    private const string GroupCharacterScreen = GroupAttributeBonuses + "/" + Texts.Ids.GroupCharacterScreen;
    private const string GroupMeleeDamage = GroupVigor + "/" + Texts.Ids.MeleeDamage;
    private const string GroupDrawSpeed = GroupVigor + "/" + Texts.Ids.DrawSpeed;
    private const string GroupCrushThrough = GroupVigor + "/" + Texts.Ids.CrushThrough;
    private const string GroupSliceThrough = GroupVigor + "/" + Texts.Ids.SliceThrough;
    private const string GroupRangedDamage = GroupControl + "/" + Texts.Ids.RangedDamage;
    private const string GroupAccuracy = GroupControl + "/" + Texts.Ids.Accuracy;
    private const string GroupReloadSpeed = GroupControl + "/" + Texts.Ids.ReloadSpeed;
    private const string GroupWeaponHandling = GroupControl + "/" + Texts.Ids.WeaponHandling;
    private const string GroupMaxHitPoints = GroupEndurance + "/" + Texts.Ids.MaxHitPoints;
    private const string GroupStaggerResistance = GroupEndurance + "/" + Texts.Ids.StaggerResistance;
    private const string GroupAimStability = GroupEndurance + "/" + Texts.Ids.AimStability;
    private const string GroupMovementSpeed = GroupEndurance + "/" + Texts.Ids.MovementSpeed;
    private const string GroupSmithingStamina = GroupEndurance + "/" + Texts.Ids.SmithingStamina;
    private const string GroupSimulationAdvantage = GroupCunning + "/" + Texts.Ids.SimulationAdvantage;
    private const string GroupVictoryRenown = GroupCunning + "/" + Texts.Ids.VictoryRenown;
    private const string GroupVictoryMorale = GroupCunning + "/" + Texts.Ids.VictoryMorale;
    private const string GroupPersuasionChance = GroupSocial + "/" + Texts.Ids.PersuasionChance;
    private const string GroupPartyMorale = GroupSocial + "/" + Texts.Ids.PartyMorale;
    private const string GroupPartySize = GroupSocial + "/" + Texts.Ids.PartySize;
    private const string GroupPartyWages = GroupSocial + "/" + Texts.Ids.PartyWages;
    private const string GroupCompanionLimit = GroupSocial + "/" + Texts.Ids.CompanionLimit;
    private const string GroupClanIncome = GroupIntelligence + "/" + Texts.Ids.ClanIncome;
    private const string GroupVictoryInfluence = GroupIntelligence + "/" + Texts.Ids.VictoryInfluence;
    private const string GroupExperienceGain = GroupIntelligence + "/" + Texts.Ids.ExperienceGain;
    private const string GroupRoleExperience = GroupIntelligence + "/" + Texts.Ids.RoleExperience;
    private const string GroupHeroHealing = GroupIntelligence + "/" + Texts.Ids.HeroHealing;

    public override string Id => SettingsId;

    public override string DisplayName => Texts.ModName.ToString();

    public override string FolderName => ModInfo.ModuleId;

    public override string FormatType => SaveFormat;

    [SettingPropertyBool(Texts.Ids.ShowMessages, RequireRestart = false, HintText = Texts.Ids.ShowMessagesHint, Order = 0)]
    [SettingPropertyGroup(Texts.Ids.GroupGlobal, GroupOrder = 0)]
    public bool ShowMessages { get; set; } = ModConfiguration.ShowMessagesDefault;

    [SettingPropertyBool(Texts.Ids.DisableSafeguards, RequireRestart = false, HintText = Texts.Ids.DisableSafeguardsHint, Order = 1)]
    [SettingPropertyGroup(Texts.Ids.GroupGlobal, GroupOrder = 0)]
    public bool DisableSafeguards { get; set; } = ModConfiguration.DisableSafeguardsDefault;

    [SettingPropertyBool(Texts.Ids.ShowSkillLimit, RequireRestart = false, HintText = Texts.Ids.ShowSkillLimitHint, Order = 2)]
    [SettingPropertyGroup(Texts.Ids.GroupGlobal, GroupOrder = 0)]
    public bool ShowSkillLimit { get; set; } = ModConfiguration.ShowSkillLimitDefault;

    [SettingPropertyBool(Texts.Ids.ShowHeroListMarkers, RequireRestart = false, HintText = Texts.Ids.ShowHeroListMarkersHint, Order = 3)]
    [SettingPropertyGroup(Texts.Ids.GroupGlobal, GroupOrder = 0)]
    public bool ShowHeroListMarkers { get; set; } = ModConfiguration.ShowHeroListMarkersDefault;

    [SettingPropertyInteger(Texts.Ids.PlayerMasteryLevelOffset, ModConfiguration.MasteryLevelOffsetMin, ModConfiguration.MasteryLevelOffsetMax, WholeNumberFormat, RequireRestart = false, HintText = Texts.Ids.PlayerMasteryLevelOffsetHint, Order = 0)]
    [SettingPropertyGroup(Texts.Ids.GroupPlayer, GroupOrder = 1)]
    public int PlayerMasteryLevelOffset { get; set; } = ModConfiguration.MasteryLevelOffsetDefault;

    [SettingPropertyInteger(Texts.Ids.PlayerMasteryStartingLevelOffset, ModConfiguration.MasteryStartingLevelOffsetMin, ModConfiguration.MasteryStartingLevelOffsetMax, WholeNumberFormat, RequireRestart = false, HintText = Texts.Ids.PlayerMasteryStartingLevelOffsetHint, Order = 1)]
    [SettingPropertyGroup(Texts.Ids.GroupPlayer, GroupOrder = 1)]
    public int PlayerMasteryStartingLevelOffset { get; set; } = ModConfiguration.MasteryStartingLevelOffsetDefault;

    [SettingPropertyBool(Texts.Ids.AutoAssignPlayer, RequireRestart = false, HintText = Texts.Ids.AutoAssignPlayerHint, Order = 2)]
    [SettingPropertyGroup(Texts.Ids.GroupPlayer, GroupOrder = 1)]
    public bool AutoAssignPlayer { get; set; } = ModConfiguration.AutoAssignPlayerDefault;

    [SettingPropertyBool(Texts.Ids.AllowCompanions, RequireRestart = false, HintText = Texts.Ids.AllowCompanionsHint, Order = 0)]
    [SettingPropertyGroup(Texts.Ids.GroupCompanion, GroupOrder = 2)]
    public bool AllowCompanions { get; set; } = ModConfiguration.AllowCompanionsDefault;

    [SettingPropertyInteger(Texts.Ids.CompanionMasteryLevelOffset, ModConfiguration.MasteryLevelOffsetMin, ModConfiguration.MasteryLevelOffsetMax, WholeNumberFormat, RequireRestart = false, HintText = Texts.Ids.CompanionMasteryLevelOffsetHint, Order = 1)]
    [SettingPropertyGroup(Texts.Ids.GroupCompanion, GroupOrder = 2)]
    public int CompanionMasteryLevelOffset { get; set; } = ModConfiguration.MasteryLevelOffsetDefault;

    [SettingPropertyInteger(Texts.Ids.CompanionMasteryStartingLevelOffset, ModConfiguration.MasteryStartingLevelOffsetMin, ModConfiguration.MasteryStartingLevelOffsetMax, WholeNumberFormat, RequireRestart = false, HintText = Texts.Ids.CompanionMasteryStartingLevelOffsetHint, Order = 2)]
    [SettingPropertyGroup(Texts.Ids.GroupCompanion, GroupOrder = 2)]
    public int CompanionMasteryStartingLevelOffset { get; set; } = ModConfiguration.MasteryStartingLevelOffsetDefault;

    [SettingPropertyBool(Texts.Ids.AutoAssignCompanions, RequireRestart = false, HintText = Texts.Ids.AutoAssignCompanionsHint, Order = 3)]
    [SettingPropertyGroup(Texts.Ids.GroupCompanion, GroupOrder = 2)]
    public bool AutoAssignCompanions { get; set; } = ModConfiguration.AutoAssignCompanionsDefault;

    [SettingPropertyBool(Texts.Ids.AllowAILords, RequireRestart = false, HintText = Texts.Ids.AllowAILordsHint, Order = 0)]
    [SettingPropertyGroup(Texts.Ids.GroupAILord, GroupOrder = 3)]
    public bool AllowAILords { get; set; } = ModConfiguration.AllowAILordsDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.MinimumLearningRate, ModConfiguration.MinimumLearningRateMin, ModConfiguration.MinimumLearningRateMax, TwoDecimalsFormat, RequireRestart = false, HintText = Texts.Ids.MinimumLearningRateHint, Order = 1)]
    [SettingPropertyGroup(Texts.Ids.GroupLearning, GroupOrder = 4)]
    public float MinimumLearningRate { get; set; } = ModConfiguration.MinimumLearningRateDefault;

    #region Attribute bonuses
    // MCM gives a group its order only when the group is created by one of its own settings, so every group's own settings
    // are declared before its subgroups' settings (the attribute toggles exist for that too).
    [SettingPropertyBool(Texts.Ids.EnableAttributeBonuses, RequireRestart = false, HintText = Texts.Ids.EnableAttributeBonusesHint, Order = 0)]
    [SettingPropertyGroup(GroupAttributeBonuses, GroupOrder = 5)]
    public bool EnableAttributeBonuses { get; set; } = ModConfiguration.EnableAttributeBonusesDefault;

    [SettingPropertyBool(Texts.Ids.OverwriteBetterAttributes, RequireRestart = true, HintText = Texts.Ids.OverwriteBetterAttributesHint, Order = 1)]
    [SettingPropertyGroup(GroupAttributeBonuses, GroupOrder = 5)]
    public bool OverwriteBetterAttributes { get; set; } = ModConfiguration.OverwriteBetterAttributesDefault;

    // Vigor
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AttributeGroupEnabledHint, Order = 0)]
    [SettingPropertyGroup(GroupVigor, GroupOrder = 0)]
    public bool VigorBonuses { get; set; } = ModConfiguration.AttributeGroupEnabledDefault;

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.MeleeDamageHint, Order = 0)]
    [SettingPropertyGroup(GroupMeleeDamage, GroupOrder = 0)]
    public bool MeleeDamage { get; set; } = ModConfiguration.MeleeDamageDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.MeleeDamagePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupMeleeDamage, GroupOrder = 0)]
    public float MeleeDamagePerPoint { get; set; } = ModConfiguration.MeleeDamagePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupMeleeDamage, GroupOrder = 0)]
    public Dropdown<string> MeleeDamageRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.DrawSpeedHint, Order = 0)]
    [SettingPropertyGroup(GroupDrawSpeed, GroupOrder = 1)]
    public bool DrawSpeed { get; set; } = ModConfiguration.DrawSpeedDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.DrawSpeedPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupDrawSpeed, GroupOrder = 1)]
    public float DrawSpeedPerPoint { get; set; } = ModConfiguration.DrawSpeedPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupDrawSpeed, GroupOrder = 1)]
    public Dropdown<string> DrawSpeedRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.CrushThroughHint, Order = 0)]
    [SettingPropertyGroup(GroupCrushThrough, GroupOrder = 2)]
    public bool CrushThrough { get; set; } = ModConfiguration.CrushThroughDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.CrushThroughPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupCrushThrough, GroupOrder = 2)]
    public float CrushThroughPerPoint { get; set; } = ModConfiguration.CrushThroughPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupCrushThrough, GroupOrder = 2)]
    public Dropdown<string> CrushThroughRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.SliceThroughHint, Order = 0)]
    [SettingPropertyGroup(GroupSliceThrough, GroupOrder = 3)]
    public bool SliceThrough { get; set; } = ModConfiguration.SliceThroughDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.SliceThroughPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupSliceThrough, GroupOrder = 3)]
    public float SliceThroughPerPoint { get; set; } = ModConfiguration.SliceThroughPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupSliceThrough, GroupOrder = 3)]
    public Dropdown<string> SliceThroughRecipients { get; set; } = NewRecipients();

    // Control
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AttributeGroupEnabledHint, Order = 0)]
    [SettingPropertyGroup(GroupControl, GroupOrder = 1)]
    public bool ControlBonuses { get; set; } = ModConfiguration.AttributeGroupEnabledDefault;

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.RangedDamageHint, Order = 0)]
    [SettingPropertyGroup(GroupRangedDamage, GroupOrder = 0)]
    public bool RangedDamage { get; set; } = ModConfiguration.RangedDamageDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.RangedDamagePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupRangedDamage, GroupOrder = 0)]
    public float RangedDamagePerPoint { get; set; } = ModConfiguration.RangedDamagePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupRangedDamage, GroupOrder = 0)]
    public Dropdown<string> RangedDamageRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AccuracyHint, Order = 0)]
    [SettingPropertyGroup(GroupAccuracy, GroupOrder = 1)]
    public bool Accuracy { get; set; } = ModConfiguration.AccuracyDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.AccuracyPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupAccuracy, GroupOrder = 1)]
    public float AccuracyPerPoint { get; set; } = ModConfiguration.AccuracyPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupAccuracy, GroupOrder = 1)]
    public Dropdown<string> AccuracyRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.ReloadSpeedHint, Order = 0)]
    [SettingPropertyGroup(GroupReloadSpeed, GroupOrder = 2)]
    public bool ReloadSpeed { get; set; } = ModConfiguration.ReloadSpeedDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.ReloadSpeedPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupReloadSpeed, GroupOrder = 2)]
    public float ReloadSpeedPerPoint { get; set; } = ModConfiguration.ReloadSpeedPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupReloadSpeed, GroupOrder = 2)]
    public Dropdown<string> ReloadSpeedRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.WeaponHandlingHint, Order = 0)]
    [SettingPropertyGroup(GroupWeaponHandling, GroupOrder = 3)]
    public bool WeaponHandling { get; set; } = ModConfiguration.WeaponHandlingDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.WeaponHandlingPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupWeaponHandling, GroupOrder = 3)]
    public float WeaponHandlingPerPoint { get; set; } = ModConfiguration.WeaponHandlingPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupWeaponHandling, GroupOrder = 3)]
    public Dropdown<string> WeaponHandlingRecipients { get; set; } = NewRecipients();

    // Endurance
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AttributeGroupEnabledHint, Order = 0)]
    [SettingPropertyGroup(GroupEndurance, GroupOrder = 2)]
    public bool EnduranceBonuses { get; set; } = ModConfiguration.AttributeGroupEnabledDefault;

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.MaxHitPointsHint, Order = 0)]
    [SettingPropertyGroup(GroupMaxHitPoints, GroupOrder = 0)]
    public bool MaxHitPoints { get; set; } = ModConfiguration.MaxHitPointsDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.MaxHitPointsPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupMaxHitPoints, GroupOrder = 0)]
    public float MaxHitPointsPerPoint { get; set; } = ModConfiguration.MaxHitPointsPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupMaxHitPoints, GroupOrder = 0)]
    public Dropdown<string> MaxHitPointsRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.StaggerResistanceHint, Order = 0)]
    [SettingPropertyGroup(GroupStaggerResistance, GroupOrder = 1)]
    public bool StaggerResistance { get; set; } = ModConfiguration.StaggerResistanceDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.StaggerResistancePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupStaggerResistance, GroupOrder = 1)]
    public float StaggerResistancePerPoint { get; set; } = ModConfiguration.StaggerResistancePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupStaggerResistance, GroupOrder = 1)]
    public Dropdown<string> StaggerResistanceRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AimStabilityHint, Order = 0)]
    [SettingPropertyGroup(GroupAimStability, GroupOrder = 2)]
    public bool AimStability { get; set; } = ModConfiguration.AimStabilityDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.AimStabilityPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupAimStability, GroupOrder = 2)]
    public float AimStabilityPerPoint { get; set; } = ModConfiguration.AimStabilityPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupAimStability, GroupOrder = 2)]
    public Dropdown<string> AimStabilityRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.MovementSpeedHint, Order = 0)]
    [SettingPropertyGroup(GroupMovementSpeed, GroupOrder = 3)]
    public bool MovementSpeed { get; set; } = ModConfiguration.MovementSpeedDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.MovementSpeedPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupMovementSpeed, GroupOrder = 3)]
    public float MovementSpeedPerPoint { get; set; } = ModConfiguration.MovementSpeedPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupMovementSpeed, GroupOrder = 3)]
    public Dropdown<string> MovementSpeedRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.SmithingStaminaHint, Order = 0)]
    [SettingPropertyGroup(GroupSmithingStamina, GroupOrder = 4)]
    public bool SmithingStamina { get; set; } = ModConfiguration.SmithingStaminaDefault;

    [SettingPropertyInteger(Texts.Ids.BonusPerPoint, ModConfiguration.SmithingStaminaPerPointMin, ModConfiguration.SmithingStaminaPerPointMax, WholeNumberFormat, RequireRestart = false, HintText = Texts.Ids.SmithingStaminaPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupSmithingStamina, GroupOrder = 4)]
    public int SmithingStaminaPerPoint { get; set; } = ModConfiguration.SmithingStaminaPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupSmithingStamina, GroupOrder = 4)]
    public Dropdown<string> SmithingStaminaRecipients { get; set; } = NewRecipients();

    // Cunning
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AttributeGroupEnabledHint, Order = 0)]
    [SettingPropertyGroup(GroupCunning, GroupOrder = 3)]
    public bool CunningBonuses { get; set; } = ModConfiguration.AttributeGroupEnabledDefault;

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.SimulationAdvantageHint, Order = 0)]
    [SettingPropertyGroup(GroupSimulationAdvantage, GroupOrder = 0)]
    public bool SimulationAdvantage { get; set; } = ModConfiguration.SimulationAdvantageDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.SimulationAdvantagePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupSimulationAdvantage, GroupOrder = 0)]
    public float SimulationAdvantagePerPoint { get; set; } = ModConfiguration.SimulationAdvantagePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupSimulationAdvantage, GroupOrder = 0)]
    public Dropdown<string> SimulationAdvantageRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.VictoryRenownHint, Order = 0)]
    [SettingPropertyGroup(GroupVictoryRenown, GroupOrder = 1)]
    public bool VictoryRenown { get; set; } = ModConfiguration.VictoryRenownDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.VictoryRenownPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupVictoryRenown, GroupOrder = 1)]
    public float VictoryRenownPerPoint { get; set; } = ModConfiguration.VictoryRenownPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupVictoryRenown, GroupOrder = 1)]
    public Dropdown<string> VictoryRenownRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.VictoryMoraleHint, Order = 0)]
    [SettingPropertyGroup(GroupVictoryMorale, GroupOrder = 2)]
    public bool VictoryMorale { get; set; } = ModConfiguration.VictoryMoraleDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.VictoryMoralePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupVictoryMorale, GroupOrder = 2)]
    public float VictoryMoralePerPoint { get; set; } = ModConfiguration.VictoryMoralePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupVictoryMorale, GroupOrder = 2)]
    public Dropdown<string> VictoryMoraleRecipients { get; set; } = NewRecipients();

    // Social
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AttributeGroupEnabledHint, Order = 0)]
    [SettingPropertyGroup(GroupSocial, GroupOrder = 4)]
    public bool SocialBonuses { get; set; } = ModConfiguration.AttributeGroupEnabledDefault;

    // Player only: no "Who Gets It".
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.PersuasionChanceHint, Order = 0)]
    [SettingPropertyGroup(GroupPersuasionChance, GroupOrder = 0)]
    public bool PersuasionChance { get; set; } = ModConfiguration.PersuasionChanceDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.PersuasionChancePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupPersuasionChance, GroupOrder = 0)]
    public float PersuasionChancePerPoint { get; set; } = ModConfiguration.PersuasionChancePerPointDefault;

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.PartyMoraleHint, Order = 0)]
    [SettingPropertyGroup(GroupPartyMorale, GroupOrder = 1)]
    public bool PartyMorale { get; set; } = ModConfiguration.PartyMoraleDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.PartyMoralePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupPartyMorale, GroupOrder = 1)]
    public float PartyMoralePerPoint { get; set; } = ModConfiguration.PartyMoralePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupPartyMorale, GroupOrder = 1)]
    public Dropdown<string> PartyMoraleRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.PartySizeHint, Order = 0)]
    [SettingPropertyGroup(GroupPartySize, GroupOrder = 2)]
    public bool PartySize { get; set; } = ModConfiguration.PartySizeDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.PartySizePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupPartySize, GroupOrder = 2)]
    public float PartySizePerPoint { get; set; } = ModConfiguration.PartySizePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupPartySize, GroupOrder = 2)]
    public Dropdown<string> PartySizeRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.PartyWagesHint, Order = 0)]
    [SettingPropertyGroup(GroupPartyWages, GroupOrder = 3)]
    public bool PartyWages { get; set; } = ModConfiguration.PartyWagesDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.PartyWagesPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupPartyWages, GroupOrder = 3)]
    public float PartyWagesPerPoint { get; set; } = ModConfiguration.PartyWagesPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupPartyWages, GroupOrder = 3)]
    public Dropdown<string> PartyWagesRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.CompanionLimitHint, Order = 0)]
    [SettingPropertyGroup(GroupCompanionLimit, GroupOrder = 4)]
    public bool CompanionLimit { get; set; } = ModConfiguration.CompanionLimitDefault;

    [SettingPropertyInteger(Texts.Ids.PointsPerCompanion, ModConfiguration.CompanionLimitPointsPerCompanionMin, ModConfiguration.CompanionLimitPointsPerCompanionMax, WholeNumberFormat, RequireRestart = false, HintText = Texts.Ids.CompanionLimitPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupCompanionLimit, GroupOrder = 4)]
    public int CompanionLimitPointsPerCompanion { get; set; } = ModConfiguration.CompanionLimitPointsPerCompanionDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupCompanionLimit, GroupOrder = 4)]
    public Dropdown<string> CompanionLimitRecipients { get; set; } = NewRecipients();

    // Intelligence
    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.AttributeGroupEnabledHint, Order = 0)]
    [SettingPropertyGroup(GroupIntelligence, GroupOrder = 5)]
    public bool IntelligenceBonuses { get; set; } = ModConfiguration.AttributeGroupEnabledDefault;

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.ClanIncomeHint, Order = 0)]
    [SettingPropertyGroup(GroupClanIncome, GroupOrder = 0)]
    public bool ClanIncome { get; set; } = ModConfiguration.ClanIncomeDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.ClanIncomePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupClanIncome, GroupOrder = 0)]
    public float ClanIncomePerPoint { get; set; } = ModConfiguration.ClanIncomePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupClanIncome, GroupOrder = 0)]
    public Dropdown<string> ClanIncomeRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.VictoryInfluenceHint, Order = 0)]
    [SettingPropertyGroup(GroupVictoryInfluence, GroupOrder = 1)]
    public bool VictoryInfluence { get; set; } = ModConfiguration.VictoryInfluenceDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.VictoryInfluencePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupVictoryInfluence, GroupOrder = 1)]
    public float VictoryInfluencePerPoint { get; set; } = ModConfiguration.VictoryInfluencePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupVictoryInfluence, GroupOrder = 1)]
    public Dropdown<string> VictoryInfluenceRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.ExperienceGainHint, Order = 0)]
    [SettingPropertyGroup(GroupExperienceGain, GroupOrder = 2)]
    public bool ExperienceGain { get; set; } = ModConfiguration.ExperienceGainDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.ExperienceGainPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupExperienceGain, GroupOrder = 2)]
    public float ExperienceGainPerPoint { get; set; } = ModConfiguration.ExperienceGainPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupExperienceGain, GroupOrder = 2)]
    public Dropdown<string> ExperienceGainRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.RoleExperienceHint, Order = 0)]
    [SettingPropertyGroup(GroupRoleExperience, GroupOrder = 3)]
    public bool RoleExperience { get; set; } = ModConfiguration.RoleExperienceDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.RoleExperiencePerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupRoleExperience, GroupOrder = 3)]
    public float RoleExperiencePerPoint { get; set; } = ModConfiguration.RoleExperiencePerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupRoleExperience, GroupOrder = 3)]
    public Dropdown<string> RoleExperienceRecipients { get; set; } = NewRecipients();

    [SettingPropertyBool(Texts.Ids.BonusEnabled, IsToggle = true, RequireRestart = false, HintText = Texts.Ids.HeroHealingHint, Order = 0)]
    [SettingPropertyGroup(GroupHeroHealing, GroupOrder = 4)]
    public bool HeroHealing { get; set; } = ModConfiguration.HeroHealingDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.BonusPerPoint, ModConfiguration.BonusPerPointMin, ModConfiguration.BonusPerPointMax, PercentFormat, RequireRestart = false, HintText = Texts.Ids.HeroHealingPerPointHint, Order = 1)]
    [SettingPropertyGroup(GroupHeroHealing, GroupOrder = 4)]
    public float HeroHealingPerPoint { get; set; } = ModConfiguration.HeroHealingPerPointDefault;

    [SettingPropertyDropdown(Texts.Ids.BonusRecipients, RequireRestart = false, HintText = Texts.Ids.BonusRecipientsHint, Order = 2)]
    [SettingPropertyGroup(GroupHeroHealing, GroupOrder = 4)]
    public Dropdown<string> HeroHealingRecipients { get; set; } = NewRecipients();

    // Character screen
    [SettingPropertyBool(Texts.Ids.ShowOnlyBonusesInAttributeTooltip, RequireRestart = false, HintText = Texts.Ids.ShowOnlyBonusesInAttributeTooltipHint, Order = 0)]
    [SettingPropertyGroup(GroupCharacterScreen, GroupOrder = 6)]
    public bool ShowOnlyBonusesInAttributeTooltip { get; set; } = ModConfiguration.ShowOnlyBonusesInAttributeTooltipDefault;
    #endregion

    // "Technical settings" is always the last group and "Logging for debugging" its last entry.
    [SettingPropertyBool(Texts.Ids.LoggingEnabled, RequireRestart = false, HintText = Texts.Ids.LoggingEnabledHint, Order = 0)]
    [SettingPropertyGroup(Texts.Ids.GroupTechnical, GroupOrder = 6)]
    public bool LoggingEnabled { get; set; } = ModConfiguration.LoggingEnabledDefault;

    internal BonusSetting GetBonus(AttributeBonus bonus) => bonus switch
    {
        AttributeBonus.MeleeDamage => new(VigorBonuses && MeleeDamage, MeleeDamagePerPoint, Who(MeleeDamageRecipients)),
        AttributeBonus.DrawSpeed => new(VigorBonuses && DrawSpeed, DrawSpeedPerPoint, Who(DrawSpeedRecipients)),
        AttributeBonus.CrushThrough => new(VigorBonuses && CrushThrough, CrushThroughPerPoint, Who(CrushThroughRecipients)),
        AttributeBonus.SliceThrough => new(VigorBonuses && SliceThrough, SliceThroughPerPoint, Who(SliceThroughRecipients)),
        AttributeBonus.RangedDamage => new(ControlBonuses && RangedDamage, RangedDamagePerPoint, Who(RangedDamageRecipients)),
        AttributeBonus.Accuracy => new(ControlBonuses && Accuracy, AccuracyPerPoint, Who(AccuracyRecipients)),
        AttributeBonus.ReloadSpeed => new(ControlBonuses && ReloadSpeed, ReloadSpeedPerPoint, Who(ReloadSpeedRecipients)),
        AttributeBonus.WeaponHandling => new(ControlBonuses && WeaponHandling, WeaponHandlingPerPoint, Who(WeaponHandlingRecipients)),
        AttributeBonus.MaxHitPoints => new(EnduranceBonuses && MaxHitPoints, MaxHitPointsPerPoint, Who(MaxHitPointsRecipients)),
        AttributeBonus.StaggerResistance => new(EnduranceBonuses && StaggerResistance, StaggerResistancePerPoint, Who(StaggerResistanceRecipients)),
        AttributeBonus.AimStability => new(EnduranceBonuses && AimStability, AimStabilityPerPoint, Who(AimStabilityRecipients)),
        AttributeBonus.MovementSpeed => new(EnduranceBonuses && MovementSpeed, MovementSpeedPerPoint, Who(MovementSpeedRecipients)),
        AttributeBonus.SmithingStamina => new(EnduranceBonuses && SmithingStamina, SmithingStaminaPerPoint, Who(SmithingStaminaRecipients)),
        AttributeBonus.SimulationAdvantage => new(CunningBonuses && SimulationAdvantage, SimulationAdvantagePerPoint, Who(SimulationAdvantageRecipients)),
        AttributeBonus.VictoryRenown => new(CunningBonuses && VictoryRenown, VictoryRenownPerPoint, Who(VictoryRenownRecipients)),
        AttributeBonus.VictoryMorale => new(CunningBonuses && VictoryMorale, VictoryMoralePerPoint, Who(VictoryMoraleRecipients)),
        // Only the player persuades.
        AttributeBonus.PersuasionChance => new(SocialBonuses && PersuasionChance, PersuasionChancePerPoint, BonusRecipients.Player),
        AttributeBonus.PartyMorale => new(SocialBonuses && PartyMorale, PartyMoralePerPoint, Who(PartyMoraleRecipients)),
        AttributeBonus.PartySize => new(SocialBonuses && PartySize, PartySizePerPoint, Who(PartySizeRecipients)),
        AttributeBonus.PartyWages => new(SocialBonuses && PartyWages, PartyWagesPerPoint, Who(PartyWagesRecipients)),
        AttributeBonus.CompanionLimit => new(SocialBonuses && CompanionLimit, CompanionLimitPointsPerCompanion, Who(CompanionLimitRecipients)),
        AttributeBonus.ClanIncome => new(IntelligenceBonuses && ClanIncome, ClanIncomePerPoint, Who(ClanIncomeRecipients)),
        AttributeBonus.VictoryInfluence => new(IntelligenceBonuses && VictoryInfluence, VictoryInfluencePerPoint, Who(VictoryInfluenceRecipients)),
        AttributeBonus.ExperienceGain => new(IntelligenceBonuses && ExperienceGain, ExperienceGainPerPoint, Who(ExperienceGainRecipients)),
        AttributeBonus.RoleExperience => new(IntelligenceBonuses && RoleExperience, RoleExperiencePerPoint, Who(RoleExperienceRecipients)),
        AttributeBonus.HeroHealing => new(IntelligenceBonuses && HeroHealing, HeroHealingPerPoint, Who(HeroHealingRecipients)),
        _ => default,
    };

    // Option order = BonusRecipients values.
    private static Dropdown<string> NewRecipients() =>
        new([Texts.Ids.RecipientPlayer, Texts.Ids.RecipientPlayerClan, Texts.Ids.RecipientAllHeroes], (int)ModConfiguration.BonusRecipientsDefault);

    private static BonusRecipients Who(Dropdown<string> dropdown) =>
        dropdown.SelectedIndex is >= (int)BonusRecipients.Player and <= (int)BonusRecipients.AllHeroes
            ? (BonusRecipients)dropdown.SelectedIndex
            : ModConfiguration.BonusRecipientsDefault;
}
