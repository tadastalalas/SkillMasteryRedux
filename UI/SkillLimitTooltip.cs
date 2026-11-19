using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core.ViewModelCollection.Information;

namespace SkillMasteryRedux.UI;

// Wraps SkillVM.LearningLimitTooltip so it ends with a "Skill Limit" row (level where the learning rate reaches zero).
// Decided at show time, so attribute/focus changes made in the screen are reflected without re-installing
// (Show Skill Limit re-created the whole tooltip VM every frame).
internal sealed class SkillLimitTooltip
{
    // Fragile: private vanilla name, re-verify in dnSpy on game updates.
    private static readonly FieldInfo? TooltipPropertiesField = AccessTools.Field(typeof(BasicTooltipViewModel), "_tooltipProperties");

    private readonly CharacterDeveloperHeroItemVM _character;
    private readonly SkillVM _skill;
    private readonly Func<List<TooltipProperty>> _vanilla;

    private SkillLimitTooltip(CharacterDeveloperHeroItemVM character, SkillVM skill, Func<List<TooltipProperty>> vanilla)
    {
        _character = character;
        _skill = skill;
        _vanilla = vanilla;
    }

    // Idempotent; SkillLimitBar calls it only when the tooltip instance changes.
    internal static void Install(BasicTooltipViewModel hint, CharacterDeveloperHeroItemVM character, SkillVM skill)
    {
        if (TooltipPropertiesField == null)
        {
            SmrLog.WarnOnce("BasicTooltipViewModel._tooltipProperties:SkillLimit", "BasicTooltipViewModel._tooltipProperties not found; skill limit tooltip row inert.");
            return;
        }
        if (TooltipPropertiesField.GetValue(hint) is not Func<List<TooltipProperty>> vanilla || vanilla.Target is SkillLimitTooltip) return;
        TooltipPropertiesField.SetValue(hint, new Func<List<TooltipProperty>>(new SkillLimitTooltip(character, skill, vanilla).Build));
    }

    private List<TooltipProperty> Build()
    {
        List<TooltipProperty> rows = _vanilla();
        if (rows == null || !MasteryRules.ShowSkillLimit) return rows;
        rows.Add(new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
        rows.Add(new TooltipProperty(Texts.MinimumLearningRateFrom.ToString(), SkillLimitBar.SkillLimit(_character, _skill).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.RundownResult));
        return rows;
    }
}