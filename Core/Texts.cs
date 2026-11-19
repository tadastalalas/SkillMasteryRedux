using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SkillMasteryRedux.Core;

internal static class Texts
{
    #region Settings
    public static readonly TextObject ModName = new(Ids.ModName);
    #endregion

    #region Mastery messages
    public static TextObject PlayerMastered(SkillObject skill) => new TextObject(Ids.PlayerMastered).SetTextVariable(Vars.Skill, skill.Name);

    public static TextObject PlayerMasteryGrows() => new(Ids.PlayerMasteryGrows);

    public static TextObject HeroMastered(TextObject hero, SkillObject skill) => new TextObject(Ids.HeroMastered).SetTextVariable(Vars.Hero, hero).SetTextVariable(Vars.Skill, skill.Name);

    public static TextObject HeroMasteryGrows(TextObject hero) => new TextObject(Ids.HeroMasteryGrows).SetTextVariable(Vars.Hero, hero);
    #endregion

    #region Tooltips
    public static readonly TextObject MasterySkillLevel = new(Ids.MasterySkillLevel);
    public static readonly TextObject MinimumLearningRateFrom = new(Ids.MinimumLearningRateFrom);
    #endregion

    internal static class Vars
    {
        public const string Skill = "SKILL";
        public const string Hero = "HERO";
    }

    internal static class Ids
    {
        #region Settings
        public const string ModName = "{=SMR_Kq7vXe2}Skill Mastery Redux";
        public const string GroupGlobal = "{=SMR_3wLpR9t}Global Settings";
        public const string ShowMessages = "{=SMR_pDsW4n7}Show Mastery Messages";
        public const string ShowMessagesHint = "{=SMR_Gxa9Le3}Show an on-screen message when a mastery perk is unlocked in the character screen and when a skill is fully mastered. [Default: enabled]";
        public const string DisableSafeguards = "{=SMR_zBtH6q8}Disable Perk Safeguards";
        public const string DisableSafeguardsHint = "{=SMR_eMkP1y5}Removes the cap that keeps every mastery perk reachable at or below skill level 330. Skills keep learning at the minimum learning rate past 330, so higher tiers stay reachable, just later. [Default: disabled]";
        public const string ShowSkillLimit = "{=SMR_Hq4vTz8}Show Skill Limit on Skill Bar";
        public const string ShowSkillLimitHint = "{=SMR_pLw7Rn2}Marks the level from which a skill only learns at the minimum learning rate (vanilla stops there) on the character screen skill bar and adds it to the learning limit tooltip. [Default: enabled]";
        public const string ShowHeroListMarkers = "{=SMR_Ua6nWc3}Show Unspent Points in Hero List";
        public const string ShowHeroListMarkersHint = "{=SMR_Jf8rLm5}Marks heroes in the character screen hero list: an orange circle for unspent attribute or focus points and a shield with the number of perks to choose. Applies the next time the character screen opens. [Default: enabled]";
        public const string GroupPlayer = "{=SMR_hNc8Bz4}Player Settings";
        public const string PlayerMasteryLevelOffset = "{=SMR_VcrJ7u0}Mastery Perk Cost Offset";
        public const string PlayerMasteryLevelOffsetHint = "{=SMR_oSn3Zf9}Extra skill levels required per perk tier for the player's mastery perks. With safeguards enabled it is reduced so the last tier stays at or below 330 (8/5/2 for 250/275/300 skills at the default starting offset). [Default: 20]";
        public const string PlayerMasteryStartingLevelOffset = "{=SMR_LwdK8a2}Mastery Starting Level Offset";
        public const string PlayerMasteryStartingLevelOffsetHint = "{=SMR_jTgE4b6}Shifts the skill level at which the player's mastery perks begin. Keep the cost offset at 20 when lowering this so the tiers still scale up to 330. [Default: 0]";
        public const string AutoAssignPlayer = "{=SMR_QhyN2v1}Auto-assign Mastery Perks";
        public const string AutoAssignPlayerHint = "{=SMR_bRxC9m4}Available mastery perks for the player are acquired automatically when the character screen refreshes. [Default: disabled]";
        public const string GroupCompanion = "{=SMR_YtQ5md1}Companion Settings";
        public const string AllowCompanions = "{=SMR_fKuD5s7}Allow Companions to Master Skills";
        public const string AllowCompanionsHint = "{=SMR_XnpG1w3}Heroes of your clan can unlock mastery perks. If disabled, only the player can. [Default: enabled]";
        public const string CompanionMasteryLevelOffset = "{=SMR_mZeT6k8}Mastery Perk Cost Offset";
        public const string CompanionMasteryLevelOffsetHint = "{=SMR_cJvR3q5}Extra skill levels required per perk tier for companion and AI lord mastery perks. Same safeguard as the player value. [Default: 20]";
        public const string CompanionMasteryStartingLevelOffset = "{=SMR_DybL7h0}Mastery Starting Level Offset";
        public const string CompanionMasteryStartingLevelOffsetHint = "{=SMR_sPwF2n9}Shifts the skill level at which companion and AI lord mastery perks begin. [Default: 0]";
        public const string AutoAssignCompanions = "{=SMR_UakM8z4}Auto-assign Mastery Perks";
        public const string AutoAssignCompanionsHint = "{=SMR_gEqX4t1}Companions acquire available mastery perks automatically, daily and when the character screen refreshes. If disabled, you unlock them in the character screen. [Default: enabled]";
        public const string GroupAILord = "{=SMR_r2FjVk6}AI Lord Settings";
        public const string AllowAILords = "{=SMR_NvcB9r6}Allow AI Lords to Master Skills";
        public const string AllowAILordsHint = "{=SMR_kHjY3d2}Lords outside your clan acquire mastery perks automatically on their daily tick, using the companion offsets. [Default: enabled]";
        public const string GroupLearning = "{=SMR_vRk2Pq7}Learning Settings";
        public const string MinimumLearningRate = "{=SMR_tNb6Xw3}Minimum Learning Rate";
        public const string MinimumLearningRateHint = "{=SMR_cQd9Hs4}Every hero always learns a skill at least this fast, even past its learning limit. Skills are no longer capped at 330, so every mastery perk stays reachable. [Default: 0.30]";
        public const string GroupTechnical = "{=SMR_q4Tn8Yb}Technical settings";
        public const string LoggingEnabled = "{=SMR_Lx2dWf6}Logging for debugging";
        public const string LoggingEnabledHint = "{=SMR_Pz9kRc3}Logging for debugging (English only). [Default: disabled]";
        #endregion

        #region Mastery messages
        public const string PlayerMastered = "{=SMR_TbmS5w7}You are a master of {SKILL}!";
        public const string PlayerMasteryGrows = "{=SMR_xQfK1e8}Your skill mastery grows...";
        public const string HeroMastered = "{=SMR_RdnP6a3}{HERO} is a master of {SKILL}!";
        public const string HeroMasteryGrows = "{=SMR_WgzJ2c5}{HERO}'s skill mastery grows...";
        #endregion

        #region Tooltips
        public const string MasterySkillLevel = "{=SMR_bWq4Jn8}Mastery Skill Level";
        public const string MinimumLearningRateFrom = "{=SMR_Zk3cMe9}Minimum learning rate from";
        #endregion
    }
}
