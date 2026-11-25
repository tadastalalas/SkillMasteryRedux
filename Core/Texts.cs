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

    #region Attribute bonuses
    public static TextObject BonusName(AttributeBonus bonus) => new(BonusNameId(bonus));

    // "Melee Damage: +15%"
    public static string AttributeBonusLine(AttributeBonus bonus, string value) =>
        new TextObject(Ids.AttributeBonusLine).SetTextVariable(Vars.Bonus, BonusName(bonus)).SetTextVariable(Vars.Value, value).ToString();

    // "+15%"; the number already carries its sign.
    public static string Percent(string number) => new TextObject(Ids.PercentValue).SetTextVariable(Vars.Number, number).ToString();

    private static string BonusNameId(AttributeBonus bonus) => bonus switch
    {
        AttributeBonus.MeleeDamage => Ids.MeleeDamage,
        AttributeBonus.DrawSpeed => Ids.DrawSpeed,
        AttributeBonus.CrushThrough => Ids.CrushThrough,
        AttributeBonus.SliceThrough => Ids.SliceThrough,
        AttributeBonus.RangedDamage => Ids.RangedDamage,
        AttributeBonus.Accuracy => Ids.Accuracy,
        AttributeBonus.ReloadSpeed => Ids.ReloadSpeed,
        AttributeBonus.WeaponHandling => Ids.WeaponHandling,
        AttributeBonus.MaxHitPoints => Ids.MaxHitPoints,
        AttributeBonus.StaggerResistance => Ids.StaggerResistance,
        AttributeBonus.AimStability => Ids.AimStability,
        AttributeBonus.MovementSpeed => Ids.MovementSpeed,
        AttributeBonus.SmithingStamina => Ids.SmithingStamina,
        AttributeBonus.SimulationAdvantage => Ids.SimulationAdvantage,
        AttributeBonus.VictoryRenown => Ids.VictoryRenown,
        AttributeBonus.VictoryMorale => Ids.VictoryMorale,
        AttributeBonus.PersuasionChance => Ids.PersuasionChance,
        AttributeBonus.PartyMorale => Ids.PartyMorale,
        AttributeBonus.PartySize => Ids.PartySize,
        AttributeBonus.PartyWages => Ids.PartyWages,
        AttributeBonus.CompanionLimit => Ids.CompanionLimit,
        AttributeBonus.ClanIncome => Ids.ClanIncome,
        AttributeBonus.VictoryInfluence => Ids.VictoryInfluence,
        AttributeBonus.ExperienceGain => Ids.ExperienceGain,
        AttributeBonus.RoleExperience => Ids.RoleExperience,
        AttributeBonus.HeroHealing => Ids.HeroHealing,
        _ => string.Empty,
    };
    #endregion

    internal static class Vars
    {
        public const string Skill = "SKILL";
        public const string Hero = "HERO";
        public const string Bonus = "BONUS";
        public const string Value = "VALUE";
        public const string Number = "NUMBER";
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

        #region Attribute bonuses
        public const string GroupAttributeBonuses = "{=SMR_bv3zaGT}Attribute Bonuses";
        public const string EnableAttributeBonuses = "{=SMR_4LY8pjF}Enable Attribute Bonuses";
        public const string EnableAttributeBonusesHint = "{=SMR_NM3ahyQ}Every attribute point gives the bonuses listed under its attribute below. Combat and personal bonuses use the hero's own attribute, party bonuses the party leader's and clan bonuses the clan leader's. Only heroes have attributes, so troops get nothing. Works in the campaign only. Hover over an attribute in the character screen to see its bonuses. [Default: enabled]";
        public const string OverwriteBetterAttributes = "{=SMR_6DENV2C}Overwrite Better Attributes Mod";
        public const string OverwriteBetterAttributesHint = "{=SMR_q69Z4sb}If the Better Attributes mod is loaded too, switches it off so the bonuses of both mods do not add up. If disabled, both mods give their bonuses. Applies after a game restart. [Default: enabled]";
        public const string GroupVigor = "{=SMR_EbEhjZv}Vigor";
        public const string GroupControl = "{=SMR_X37MKx6}Control";
        public const string GroupEndurance = "{=SMR_QTdKXEf}Endurance";
        public const string GroupCunning = "{=SMR_2wVnQpM}Cunning";
        public const string GroupSocial = "{=SMR_PZgu2Ei}Social";
        public const string GroupIntelligence = "{=SMR_Kw37Gtg}Intelligence";
        public const string BonusEnabled = "{=SMR_KGbkBGH}Enabled";
        public const string AttributeGroupEnabledHint = "{=SMR_k5pfV5M}Turns every bonus of this attribute on or off. [Default: enabled]";
        public const string BonusPerPoint = "{=SMR_MT4CKRh}Bonus per Attribute Point";
        public const string PointsPerCompanion = "{=SMR_cZrT3Ve}Social Points per Companion";
        public const string BonusRecipients = "{=SMR_wkMqm8p}Who Gets It";
        public const string BonusRecipientsHint = "{=SMR_YTcoRdC}Player: only you. Player's Clan: you and the heroes of your clan. All Heroes: every hero, AI lords included. For party and clan bonuses this is checked on the party or clan leader. [Default: All Heroes]";
        public const string RecipientPlayer = "{=SMR_5ZpeJE8}Player";
        public const string RecipientPlayerClan = "{=SMR_L8C7L9G}Player's Clan";
        public const string RecipientAllHeroes = "{=SMR_QSAHVDX}All Heroes";
        public const string GroupCharacterScreen = "{=SMR_TLYJ7wW}Character Screen";
        public const string ShowOnlyBonusesInAttributeTooltip = "{=SMR_92o3MNh}Show Only Bonuses in Attribute Tooltip";
        public const string ShowOnlyBonusesInAttributeTooltipHint = "{=SMR_acNspfT}The attribute tooltip in the character screen lists that attribute's bonuses for the selected hero. If enabled, the list replaces the game's text about adding points, so long lists fit. If disabled, the game's text follows the list. [Default: enabled]";
        public const string AttributeBonusLine = "{=SMR_misH9Rh}{BONUS}: {VALUE}";
        public const string PercentValue = "{=SMR_bKP5HPk}{NUMBER}%";

        public const string MeleeDamage = "{=SMR_nzDLhjM}Melee Damage";
        public const string MeleeDamageHint = "{=SMR_g3fvJE6}The hero's melee weapon attacks deal more damage, counted before armor. Kicks, shield bashes and unarmed attacks are not affected. At the default 1.5% per point: +15% at Vigor 10. [Default: enabled]";
        public const string MeleeDamagePerPointHint = "{=SMR_Wx3PPXz}Melee damage per point of Vigor. [Default: 1.5%]";
        public const string DrawSpeed = "{=SMR_god8tzQ}Draw Speed";
        public const string DrawSpeedHint = "{=SMR_Pdczdep}The hero draws bows and readies thrusts and throws faster. At the default 1% per point: +10% at Vigor 10. [Default: enabled]";
        public const string DrawSpeedPerPointHint = "{=SMR_LZsrQDf}Draw speed per point of Vigor. [Default: 1.0%]";
        public const string CrushThrough = "{=SMR_PVpExdF}Crush Through Power";
        public const string CrushThroughHint = "{=SMR_HGe7Ekq}The hero's overhead swings count as stronger when the game decides if they crush through a block. As in the base game, only overhead swings can crush through. At the default 1% per point: +10% power at Vigor 10. Not compatible with Realistic Battle Mod, which has its own rules for this. [Default: disabled]";
        public const string CrushThroughPerPointHint = "{=SMR_QZ9pdQ3}Crush through power per point of Vigor. [Default: 1.0%]";
        public const string SliceThrough = "{=SMR_BEa7PP8}Slice Through Chance";
        public const string SliceThroughHint = "{=SMR_mJRSFJ9}Chance that the hero's melee swing loses no power when it cuts through an enemy, so the next enemy it reaches is hit at full strength. At the default 1% per point: 10% at Vigor 10. Not compatible with Realistic Battle Mod, which has its own rules for this. [Default: disabled]";
        public const string SliceThroughPerPointHint = "{=SMR_gfHP4ug}Slice through chance per point of Vigor. [Default: 1.0%]";
        public const string RangedDamage = "{=SMR_qH7e86V}Ranged Damage";
        public const string RangedDamageHint = "{=SMR_dzKLXtk}The hero's arrows, bolts and thrown weapons deal more damage, counted before armor. At the default 1.5% per point: +15% at Control 10. [Default: enabled]";
        public const string RangedDamagePerPointHint = "{=SMR_PRpft3m}Ranged damage per point of Control. [Default: 1.5%]";
        public const string Accuracy = "{=SMR_LiPh5gf}Accuracy";
        public const string AccuracyHint = "{=SMR_VdNrbAQ}The hero's ranged attacks spread less. At the default 2% per point: +20% at Control 10, so the spread is divided by 1.2. [Default: enabled]";
        public const string AccuracyPerPointHint = "{=SMR_Cw4qyJF}Accuracy per point of Control. [Default: 2.0%]";
        public const string ReloadSpeed = "{=SMR_4pKNqAu}Reload Speed";
        public const string ReloadSpeedHint = "{=SMR_vEqefTk}The hero reloads faster. At the default 1.5% per point: +15% at Control 10. [Default: enabled]";
        public const string ReloadSpeedPerPointHint = "{=SMR_NKCzcSR}Reload speed per point of Control. [Default: 1.5%]";
        public const string WeaponHandling = "{=SMR_XyyjKNT}Weapon Handling";
        public const string WeaponHandlingHint = "{=SMR_RdfVnN4}The hero handles weapons more nimbly (the weapon handling stat). At the default 1% per point: +10% at Control 10. [Default: enabled]";
        public const string WeaponHandlingPerPointHint = "{=SMR_PFv6xBP}Weapon handling per point of Control. [Default: 1.0%]";
        public const string MaxHitPoints = "{=SMR_4DPBDPZ}Max Hit Points";
        public const string MaxHitPointsHint = "{=SMR_rxLc57K}The hero has more hit points. At the default 2% per point: +20% at Endurance 10, which is +20 hit points on the base 100. [Default: enabled]";
        public const string MaxHitPointsPerPointHint = "{=SMR_rt9UBs6}Hit points per point of Endurance. [Default: 2.0%]";
        public const string StaggerResistance = "{=SMR_Pf3vD4d}Stagger Resistance";
        public const string StaggerResistanceHint = "{=SMR_MzWoUeV}The hero can take bigger hits without the attack in progress being interrupted. At the default 2% per point: +20% at Endurance 10. [Default: enabled]";
        public const string StaggerResistancePerPointHint = "{=SMR_hdkwK4n}Stagger resistance per point of Endurance. [Default: 2.0%]";
        public const string AimStability = "{=SMR_MJxB6AG}Aim Stability";
        public const string AimStabilityHint = "{=SMR_CzPDSJY}The hero can hold a drawn bow steady longer before the aim starts to shake. At the default 2% per point: +20% at Endurance 10. [Default: enabled]";
        public const string AimStabilityPerPointHint = "{=SMR_Cf9NfvB}Aim stability per point of Endurance. [Default: 2.0%]";
        public const string MovementSpeed = "{=SMR_pysyoLA}Movement Speed";
        public const string MovementSpeedHint = "{=SMR_ezNAZWu}The hero moves faster on foot. At the default 0.5% per point: +5% at Endurance 10. [Default: enabled]";
        public const string MovementSpeedPerPointHint = "{=SMR_Dfo2Lya}Movement speed per point of Endurance. [Default: 0.5%]";
        public const string SmithingStamina = "{=SMR_jG5wrbR}Smithing Stamina";
        public const string SmithingStaminaHint = "{=SMR_wEYKNqD}The hero has more maximum smithing stamina. At the default 5 per point: +50 at Endurance 10 (the base game gives 100 plus half the Smithing skill). [Default: enabled]";
        public const string SmithingStaminaPerPointHint = "{=SMR_k4i6zEk}Smithing stamina per point of Endurance. [Default: 5]";
        public const string SimulationAdvantage = "{=SMR_NaPzAoo}Simulation Advantage";
        public const string SimulationAdvantageHint = "{=SMR_PbofDeR}In simulated battles (sending troops, and battles fought without you), every troop of the party deals more damage. Uses the party leader's Cunning. At the default 1% per point: +10% at Cunning 10. [Default: enabled]";
        public const string SimulationAdvantagePerPointHint = "{=SMR_c5D6rJE}Simulated damage per point of the party leader's Cunning. [Default: 1.0%]";
        public const string VictoryRenown = "{=SMR_iRsVFqk}Renown from Victories";
        public const string VictoryRenownHint = "{=SMR_eCYuPxn}Won battles give more renown. Uses the party leader's Cunning. At the default 2% per point: +20% at Cunning 10. [Default: enabled]";
        public const string VictoryRenownPerPointHint = "{=SMR_FrdzYDm}Renown per point of the party leader's Cunning. [Default: 2.0%]";
        public const string VictoryMorale = "{=SMR_YaQNmmN}Morale from Victories";
        public const string VictoryMoraleHint = "{=SMR_MzqDeem}Won battles raise the party's morale more. Uses the party leader's Cunning. At the default 2% per point: +20% at Cunning 10. [Default: enabled]";
        public const string VictoryMoralePerPointHint = "{=SMR_zwrrfnV}Morale per point of the party leader's Cunning. [Default: 2.0%]";
        public const string PersuasionChance = "{=SMR_evcqcxE}Persuasion Chance";
        public const string PersuasionChanceHint = "{=SMR_78Kejpq}Your persuasion attempts in conversations succeed more often and fail critically less often, the same way the game's own persuasion bonuses work. Player only. At the default 1.5% per point: +15% at Social 10. [Default: enabled]";
        public const string PersuasionChancePerPointHint = "{=SMR_nT4Z467}Persuasion chance per point of your Social. [Default: 1.5%]";
        public const string PartyMorale = "{=SMR_G88n3v8}Party Morale";
        public const string PartyMoraleHint = "{=SMR_57MfXrW}The party's morale is higher. Uses the party leader's Social. At the default 1% per point: +10% at Social 10. [Default: enabled]";
        public const string PartyMoralePerPointHint = "{=SMR_ibAkwbz}Party morale per point of the party leader's Social. [Default: 1.0%]";
        public const string PartySize = "{=SMR_HScDmtg}Party Size";
        public const string PartySizeHint = "{=SMR_xzyoDh9}The party can hold more troops. Uses the party leader's Social. At the default 1% per point: +10% at Social 10. [Default: enabled]";
        public const string PartySizePerPointHint = "{=SMR_kCSXU67}Party size per point of the party leader's Social. [Default: 1.0%]";
        public const string PartyWages = "{=SMR_Q9LQuqt}Party Wages";
        public const string PartyWagesHint = "{=SMR_6XfjxgS}The party pays less in troop wages. Uses the party leader's Social. At the default 1% per point: -10% at Social 10. [Default: enabled]";
        public const string PartyWagesPerPointHint = "{=SMR_XkzW5WU}Wage reduction per point of the party leader's Social. [Default: 1.0%]";
        public const string CompanionLimit = "{=SMR_FnReYdH}Companion Limit";
        public const string CompanionLimitHint = "{=SMR_fGgHmrR}The clan can have more companions. Uses the clan leader's Social. At the default of 4 points per companion: +2 companions at Social 10. [Default: enabled]";
        public const string CompanionLimitPerPointHint = "{=SMR_E2MtsxY}Points of the clan leader's Social needed for each extra companion. [Default: 4]";
        public const string ClanIncome = "{=SMR_MLjyZSK}Clan Income";
        public const string ClanIncomeHint = "{=SMR_cMauXAj}The clan's daily income is higher. Uses the clan leader's Intelligence. At the default 1% per point: +10% at Intelligence 10. [Default: enabled]";
        public const string ClanIncomePerPointHint = "{=SMR_jKfxNHw}Clan income per point of the clan leader's Intelligence. [Default: 1.0%]";
        public const string VictoryInfluence = "{=SMR_TtHN5np}Influence from Victories";
        public const string VictoryInfluenceHint = "{=SMR_Emo2gzo}Won battles give more influence (only clans in a kingdom gain influence). Uses the party leader's Intelligence. At the default 2% per point: +20% at Intelligence 10. [Default: enabled]";
        public const string VictoryInfluencePerPointHint = "{=SMR_pLoBBUp}Influence per point of the party leader's Intelligence. [Default: 2.0%]";
        public const string ExperienceGain = "{=SMR_rNja3Fx}Experience Gain";
        public const string ExperienceGainHint = "{=SMR_92pJ6nb}The hero gains more experience. At the default 1% per point: +10% at Intelligence 10. [Default: enabled]";
        public const string ExperienceGainPerPointHint = "{=SMR_3HBuqYy}Experience per point of Intelligence. [Default: 1.0%]";
        public const string RoleExperience = "{=SMR_vzHQhka}Party Leader Experience from Roles";
        public const string RoleExperienceHint = "{=SMR_iE4UhFX}When the party's scout, engineer, surgeon or quartermaster gains experience in the skill of that role, the party leader gains a share of it too. Uses the party leader's Intelligence. At the default 2% per point: 20% of it at Intelligence 10. [Default: enabled]";
        public const string RoleExperiencePerPointHint = "{=SMR_HBc8hmL}Share of the role's experience per point of the party leader's Intelligence. [Default: 2.0%]";
        public const string HeroHealing = "{=SMR_Uqhcsx6}Hero Healing Rate";
        public const string HeroHealingHint = "{=SMR_xEEKafv}Heroes in the party heal faster. Uses the party leader's Intelligence. At the default 3% per point: +30% at Intelligence 10. [Default: disabled]";
        public const string HeroHealingPerPointHint = "{=SMR_jW2dMKa}Hero healing rate per point of the party leader's Intelligence. [Default: 3.0%]";
        #endregion
    }
}
