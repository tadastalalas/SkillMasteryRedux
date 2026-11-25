---
apply: always
---

# SkillMasteryRedux: workspace instructions (they override the global file)

## Start here
- Read `FEATURES.md` (repo root) first. It maps every feature to its gate (setting) and files, so open only the files you need.
- Keep `FEATURES.md` up to date in the same change set.

## Identity
- <Prefix>: SMR (Smr in type names). <Root> namespace and ModuleId: SkillMasteryRedux ($(MSBuildProjectName)).
  ModuleName: Skill Mastery Redux.
- Logger: `Diagnostics\SmrLog.cs` (Debug / Warn / WarnOnce / Error).
- Settings: `Core\ModSettings.cs` only (MCM when loaded, else `ModConfiguration.xml`). `Core\MasteryRules.cs` keeps facade properties
  (`ShowMessages`, `ShowSkillLimit`, `ShowHeroListMarkers`, `MinimumLearningRate`) used by patches and UI.
- Attribute bonuses: facade `Core\AttributeBonusRules.cs` (`Value`, `AddFactor`, `IsRecipient`, `AttributeOf`); one entry per bonus in
  `Core\AttributeBonus.cs` (character screen order = MCM and tooltip order). A new bonus needs: enum entry, `AttributeOf` case,
  `ModConfiguration` consts + section properties + `GetBonus` case + `Clamp`, `MCMSettings` properties + `GetBonus` case, xml entry,
  `Texts` name/hint/per-point hint + `BonusNameId` case, language files, `UI\AttributeBonusTooltip.Format` if not a "+x%" value.
- MCM groups: a group only gets its `GroupOrder` when one of its own settings creates it, so declare each group's own settings
  before its subgroups' settings (`MCMSettings` attribute toggles exist for that too).
- `Core\ModInfo.cs` holds the ModuleId, file and folder names, the Harmony and UIExtender ids and the module root path.
- Patches: `SubModule.OnSubModuleLoad` runs `PatchAll` once, then registers and enables UIExtender. Never call `PatchAll` anywhere else.
- Never attribute-patch a method whose body reads a static field of `CampaignUIHelper` (or another `beforefieldinit` UI helper):
  compiling it at module load runs that class's static initializer before a game exists and breaks it for the session.
  Patch such a method once at runtime instead (see `CharacterAttributeItemVMRefreshValuesPatch.TryApply`).
- Requires Harmony and UIExtenderEx (prefab inserts in `UI\*Prefab.cs`). MCM is optional.
- Legacy-mod detection (SkillMastery, SkillMasteryFix, Show Skill Limit, Minimum Learning Rate) stays in `SubModule.cs`.
- Better Attributes: `Compat\BetterAttributesCompat.TryApply`, called once from `SubModule.OnBeforeInitialModuleScreenSetAsRoot`
  (removes its patches by assembly, neutralizes its wage model).

## Multi-version (1.5.7, 1.4.8, 1.4.7)
- 1.4.7 / 1.4.8 behavior is the reference. 1.5.7 adaptations live only in `#else` of `#if LOWER_THAN_1_5` and must behave the same.
- `#if LOWER_THAN_1_5` blocks: `UI\HeroSelectorMarkerPrefab.cs` (XPath), `Patches\DefaultCombatSimulationModelPatch.cs` and
  `Patches\DefaultPartySizeLimitModelPatch.cs` (patch argument types).
- Not checked by the compiler, re-verify on every new game version: string-named patch targets and members in `Patches\` and `UI\`
  (`RefreshCharacterSelector`, `OnLateUpdate`, `RefreshNumOfUnopenedPerks`, `GetXPositionOfLevelOnBar`, `_onChange`, `_currentState`,
  `_getIsPerkSelected`, `_tooltipProperties`, `_dataSource`, `ComputeBlowMagnitude`, `GetBonusSuccessChance`,
  `CalculateClanIncomeInternal`, `CharacterAttributeItemVM._hero`, the property names in `UI\HeroSelectorItemVM.cs` and
  `UI\PerkVMContext.cs`, `str_required_level_perk`), the prefab XPaths, widget and brush names in `UI\*Prefab.cs`, and the
  Better Attributes type names in `Compat\BetterAttributesCompat.cs`.

## Saves
- The mod saves nothing (`MasteryBehavior.SyncData` is empty). Granted perks are vanilla perks and stay in the save.

## Kept exceptions (released identifiers, never change)
- MCM Id "SkillMasteryRedux", FolderName "SkillMasteryRedux", FormatType "json", MCM property names in `Data\MCMSettings.cs`.
- Harmony id `com.skillmasteryredux`, UIExtender id `SkillMasteryRedux`.
- Behavior class name `MasteryBehavior`.
- MCM property names of the attribute bonuses and the `ModConfiguration.xml` element names under `AttributeBonusSettings`.
- Existing text ids (`SMR_` + 7 random characters). New ids use the same form.
- UI binding names `Smr*` (`SmrHasUnspentPoints`, `SmrHasUnopenedPerks`, `SmrUnopenedPerks`, `SmrSkillLimitClip`, `SmrHeroUnreadText`).
