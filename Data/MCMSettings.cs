using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
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

    // "Technical settings" is always the last group and "Logging for debugging" its last entry.
    [SettingPropertyBool(Texts.Ids.LoggingEnabled, RequireRestart = false, HintText = Texts.Ids.LoggingEnabledHint, Order = 0)]
    [SettingPropertyGroup(Texts.Ids.GroupTechnical, GroupOrder = 5)]
    public bool LoggingEnabled { get; set; } = ModConfiguration.LoggingEnabledDefault;
}
