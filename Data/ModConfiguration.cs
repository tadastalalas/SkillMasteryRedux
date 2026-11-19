using System;

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
    public const bool LoggingEnabledDefault = false;

    public GlobalSettingsSection GlobalSettings { get; set; } = new();

    public PlayerSettingsSection PlayerSettings { get; set; } = new();

    public CompanionSettingsSection CompanionSettings { get; set; } = new();

    public AILordSettingsSection AILordSettings { get; set; } = new();

    public LearningSettingsSection LearningSettings { get; set; } = new();

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
    }

    private static int Clamp(int value, int min, int max) => Math.Min(Math.Max(value, min), max);

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

    public class TechnicalSettingsSection
    {
        // Always the last entry.
        public bool LoggingEnabled { get; set; } = LoggingEnabledDefault;
    }
}
