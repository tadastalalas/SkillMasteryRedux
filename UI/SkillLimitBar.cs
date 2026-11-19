using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using SandBox.GauntletUI;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper;
using TaleWorlds.ScreenSystem;

namespace SkillMasteryRedux.UI;

// Per-frame driver behind PerkSelectionBarWidgetPatch: sizes the SkillLimitClipPrefab segment (fill from
// max(current level, full-learning-rate level) to the skill limit, indicator at the limit) and keeps the current
// skill's learning-limit tooltip extended. Reflection is resolved once; per frame only reference compares,
// property reads and three bar-position calls remain.
internal static class SkillLimitBar
{
    // Fragile: private vanilla names, re-verify in dnSpy on game updates. Null = feature inert, logged once.
    private static readonly FieldInfo? DataSourceField = AccessTools.Field(typeof(GauntletCharacterDeveloperScreen), "_dataSource");
    private static readonly Func<PerkSelectionBarWidget, float, float>? LevelToX = CreateLevelToX();

    private static GauntletCharacterDeveloperScreen? _screen;
    private static CharacterDeveloperVM? _developer;
    private static PerkSelectionBarWidget? _bar;
    private static Widget? _clip;
    private static BasicTooltipViewModel? _hint;

    internal static void Update(PerkSelectionBarWidget bar)
    {
        Widget? clip = ResolveClip(bar);
        if (clip == null) return;
        if (LevelToX == null || !MasteryRules.ShowSkillLimit || !TryGetCurrent(out CharacterDeveloperHeroItemVM? character, out SkillVM? skill))
        {
            clip.IsVisible = false;
            return;
        }

        BasicTooltipViewModel? hint = skill.LearningLimitTooltip;
        if (!ReferenceEquals(hint, _hint))
        {
            _hint = hint;
            if (hint != null) SkillLimitTooltip.Install(hint, character, skill);
        }

        float start = Math.Max(LevelToX(bar, bar.Level), LevelToX(bar, bar.FullLearningRateLevel));
        float width = LevelToX(bar, SkillLimit(character, skill)) - start;
        bool visible = width > 0f;
        clip.IsVisible = visible;
        if (!visible) return;

        // Fill is right-aligned inside the vanilla full-rate clip; offsetting it by its own width makes it start where that clip ends.
        Widget fill = clip.Children[0];
        fill.SuggestedWidth = width;
        fill.PositionXOffset = width;
        Widget indicator = clip.Children[1];
        indicator.PositionXOffset = width + indicator.SuggestedWidth / 2f - 1f; // center the fixed-size indicator on the limit (Size.X * inverseScale == SuggestedWidth)
    }

    // Last level that still gains XP. Past the (rounded) vanilla learning limit the rate is
    // 1.25 × (0.4·attributeAverage + focus − 0.1·levelsOver), i.e. zero 4·average + 10·focus levels above it
    // (14·average + 40·focus − 10 for averages ≥ 1, the value Show Skill Limit hardcoded). Integer math, ceiling for fractional averages.
    // Hot path: no LINQ, no allocations.
    internal static int SkillLimit(CharacterDeveloperHeroItemVM character, SkillVM skill)
    {
        SkillObject skillObject = skill.Skill;
        int focus = skill.CurrentFocusLevel;
        int limit = (int)Math.Round(Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(character.CharacterAttributes, focus, skillObject, false).ResultNumber);
        CharacterAttribute[] attributes = skillObject.Attributes;
        if (attributes.Length == 0) return limit;
        int sum = 0;
        for (int i = 0; i < attributes.Length; i++) sum += character.CharacterAttributes.GetPropertyValue(attributes[i]);
        int attributeLevels = SkillLimitBalance.LevelsOverLimitPerAttributePoint * sum;
        return limit + SkillLimitBalance.LevelsOverLimitPerFocusPoint * focus + (attributeLevels + attributes.Length - 1) / attributes.Length;
    }

    // Screen, VM and widget references are dropped with the campaign.
    internal static void Reset()
    {
        _screen = null;
        _developer = null;
        _bar = null;
        _clip = null;
        _hint = null;
    }

    // Once per bar instance (one per character screen).
    private static Widget? ResolveClip(PerkSelectionBarWidget bar)
    {
        if (ReferenceEquals(bar, _bar)) return _clip;
        _bar = bar;
        Widget? clip = bar.FindChild(SkillLimitClipPrefab.ClipId, true);
        if (clip == null || clip.Children.Count < 2)
        {
            _clip = null;
            SmrLog.WarnOnce("SkillLimitClip", $"{SkillLimitClipPrefab.ClipId} not found under PerkSelectionBarWidget; UIExtenderEx extension not applied. Skill limit marker inert.");
            return null;
        }
        _clip = clip;
        return clip;
    }

    // The bar has no VM reference; the screen's data source leads to CurrentCharacter.CurrentSkill. Read once per screen instance.
    private static bool TryGetCurrent([NotNullWhen(true)] out CharacterDeveloperHeroItemVM? character, [NotNullWhen(true)] out SkillVM? skill)
    {
        character = null;
        skill = null;
        if (ScreenManager.TopScreen is not GauntletCharacterDeveloperScreen screen) return false;
        if (!ReferenceEquals(screen, _screen))
        {
            CharacterDeveloperVM? developer = DataSourceField?.GetValue(screen) as CharacterDeveloperVM;
            if (developer == null)
            {
                SmrLog.WarnOnce("GauntletCharacterDeveloperScreen._dataSource", "GauntletCharacterDeveloperScreen._dataSource not found; skill limit marker inert.");
                return false;
            }
            _screen = screen;
            _developer = developer;
        }
        character = _developer?.CurrentCharacter;
        skill = character?.CurrentSkill;
        return skill != null;
    }

    private static Func<PerkSelectionBarWidget, float, float>? CreateLevelToX()
    {
        MethodInfo? method = AccessTools.Method(typeof(PerkSelectionBarWidget), "GetXPositionOfLevelOnBar", [typeof(float)]);
        if (method == null)
        {
            SmrLog.Warn("PerkSelectionBarWidget.GetXPositionOfLevelOnBar(float) not found; skill limit marker inert.");
            return null;
        }
        try
        {
            return AccessTools.MethodDelegate<Func<PerkSelectionBarWidget, float, float>>(method);
        }
        catch (Exception ex)
        {
            SmrLog.Error("PerkSelectionBarWidget.GetXPositionOfLevelOnBar delegate failed; skill limit marker inert.", ex); // signature drift must not take the UI down
            return null;
        }
    }
}