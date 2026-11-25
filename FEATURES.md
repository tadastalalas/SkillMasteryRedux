# SkillMasteryRedux — Features

| Feature | Gate | File |
|---|---|---|
| Mastery perk becomes selectable / auto-assigned in the character screen | MasteryRules.IsEligible (+ IsAutoAssign) | `Patches\PerkVMPatch.cs` (`PerkVMRefreshStatePatch`) |
| Click unlocks an eligible mastery perk | MasteryRules.IsEligible | `Patches\PerkVMPatch.cs` (`PerkVMExecuteStartSelectionPatch`) |
| Perk tooltip shows the mastery level | IsEnabledFor + IsCandidate | `Patches\PerkVMPatch.cs` (`PerkVMCtorPatch`), `UI\MasteryTooltip.cs` |
| Unopened-perk badge counts selectable mastery perks | always | `Patches\SkillVMPatch.cs` |
| Companions and AI lords acquire mastery perks daily | MasteryRules.IsAutoMasteryHero | `Behaviors\MasteryBehavior.cs`, `Core\MasteryGrant.cs` |
| Mastery feedback messages | setting:ShowMessages | `UI\MasteryMessages.cs` |
| Mastery level formula / rules facade | — | `Core\MasteryRules.cs`, `Core\MasteryBalance.cs` |
| Legacy module detection (SkillMastery / SkillMasteryFix / Show Skill Limit / Minimum Learning Rate) | always | `SubModule.cs` |
| Skill limit marker on the character screen skill bar | setting:ShowSkillLimit | `Patches\PerkSelectionBarWidgetPatch.cs`, `UI\SkillLimitBar.cs`, `UI\SkillLimitClipPrefab.cs`, `Core\SkillLimitBalance.cs` |
| Learning limit tooltip ends with a Skill Limit row | setting:ShowSkillLimit | `UI\SkillLimitTooltip.cs`, `UI\SkillLimitHintPrefab.cs` |
| Minimum learning rate at every skill level (all heroes) | setting:MinimumLearningRate | `Patches\DefaultCharacterDevelopmentModelPatch.cs` |
| Hero dropdown marks unspent attribute/focus points and unopened perks | setting:ShowHeroListMarkers | `Patches\CharacterDeveloperVMPatch.cs`, `UI\HeroSelectorItemVM.cs`, `UI\HeroSelectorMarkerPrefab.cs`, `Core\UnopenedPerkCounter.cs` |
| Attribute bonuses: bonus list, attribute per bonus, who gets it, value per attribute point | setting:EnableAttributeBonuses + attribute toggle + bonus toggle + Who Gets It | `Core\AttributeBonusRules.cs`, `Core\AttributeBonus.cs`, `Core\BonusSetting.cs`, `Core\AttributeBonusBalance.cs`, `Data\BonusRecipients.cs` |
| Melee Damage, Slice Through Chance (Vigor) and Ranged Damage (Control) attribute bonuses | setting:MeleeDamage / SliceThrough / RangedDamage | `Patches\MissionCombatMechanicsHelperPatch.cs` |
| Crush Through Power (Vigor) and Stagger Resistance (Endurance) attribute bonuses | setting:CrushThrough / StaggerResistance | `Patches\SandboxAgentApplyDamageModelPatch.cs` |
| Draw Speed (Vigor), Accuracy, Reload Speed, Weapon Handling (Control), Aim Stability, Movement Speed (Endurance) attribute bonuses | setting per bonus | `Patches\SandboxAgentStatCalculateModelPatch.cs` |
| Max Hit Points (Endurance) attribute bonus | setting:MaxHitPoints | `Patches\DefaultCharacterStatsModelPatch.cs` |
| Smithing Stamina (Endurance) attribute bonus | setting:SmithingStamina | `Patches\CraftingCampaignBehaviorPatch.cs` |
| Simulation Advantage (Cunning) attribute bonus | setting:SimulationAdvantage | `Patches\DefaultCombatSimulationModelPatch.cs` |
| Renown / Morale from Victories (Cunning), Influence from Victories (Intelligence) attribute bonuses | setting:VictoryRenown / VictoryMorale / VictoryInfluence | `Patches\DefaultBattleRewardModelPatch.cs` |
| Persuasion Chance (Social) attribute bonus | setting:PersuasionChance | `Patches\DefaultPersuasionModelPatch.cs` |
| Party Morale (Social) attribute bonus | setting:PartyMorale | `Patches\DefaultPartyMoraleModelPatch.cs` |
| Party Size (Social) attribute bonus | setting:PartySize | `Patches\DefaultPartySizeLimitModelPatch.cs` |
| Party Wages (Social) attribute bonus | setting:PartyWages | `Patches\DefaultPartyWageModelPatch.cs` |
| Companion Limit (Social) attribute bonus | setting:CompanionLimit | `Patches\DefaultClanTierModelPatch.cs` |
| Clan Income (Intelligence) attribute bonus | setting:ClanIncome | `Patches\DefaultClanFinanceModelPatch.cs` |
| Experience Gain (Intelligence) attribute bonus | setting:ExperienceGain | `Patches\DefaultGenericXpModelPatch.cs` |
| Party Leader Experience from Roles (Intelligence) attribute bonus | setting:RoleExperience | `Patches\HeroPatch.cs` |
| Hero Healing Rate (Intelligence) attribute bonus | setting:HeroHealing | `Patches\DefaultPartyHealingModelPatch.cs` |
| Attribute tooltip in the character screen lists the attribute's bonuses | setting:EnableAttributeBonuses (+ ShowOnlyBonusesInAttributeTooltip) | `Patches\CharacterAttributeItemVMPatch.cs`, `UI\AttributeBonusTooltip.cs` |
| Overwrite Better Attributes: removes its patches and neutralizes its wage model | setting:OverwriteBetterAttributes | `Compat\BetterAttributesCompat.cs`, `SubModule.cs` |
| Settings: MCM when loaded, otherwise ModConfiguration.xml | always | `Core\ModSettings.cs`, `Data\MCMSettings.cs`, `Data\ModConfiguration.cs`, `_Module\ModConfiguration.xml` |
| Log file (problems always, debug lines only when enabled) | setting:LoggingEnabled | `Diagnostics\SmrLog.cs`, `Core\ModInfo.cs`, `SubModule.cs` (static initializer failures while patching) |
| Localization (English + 12 languages) | — | `Core\Texts.cs`, `_Module\ModuleData\Languages\` |
