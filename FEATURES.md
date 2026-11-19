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
| Settings: MCM when loaded, otherwise ModConfiguration.xml | always | `Core\ModSettings.cs`, `Data\MCMSettings.cs`, `Data\ModConfiguration.cs`, `_Module\ModConfiguration.xml` |
| Log file (problems always, debug lines only when enabled) | setting:LoggingEnabled | `Diagnostics\SmrLog.cs`, `Core\ModInfo.cs` |
| Localization (English + 12 languages) | — | `Core\Texts.cs`, `_Module\ModuleData\Languages\` |
