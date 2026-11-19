using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core;

namespace SkillMasteryRedux.UI;

// Wraps a PerkVM hint so vanilla's required-level row becomes a "Mastery Skill Level" row while the perk is a mastery candidate.
// Decided at show time (old mod froze the decision in the ctor and dropped vanilla's isActive argument).
internal sealed class MasteryTooltip
{
    // Fragile: private vanilla name, re-verify in dnSpy on game updates.
    private static readonly FieldInfo? TooltipPropertiesField = AccessTools.Field(typeof(BasicTooltipViewModel), "_tooltipProperties");

    private readonly PerkVM _vm;
    private readonly Func<List<TooltipProperty>> _vanilla;

    private MasteryTooltip(PerkVM vm, Func<List<TooltipProperty>> vanilla)
    {
        _vm = vm;
        _vanilla = vanilla;
    }

    // Idempotent; safe to call from every RefreshState in case vanilla rebuilds Hint.
    internal static void Install(PerkVM vm)
    {
        BasicTooltipViewModel? hint = vm.Hint;
        if (hint == null) return;
        if (TooltipPropertiesField == null)
        {
            SmrLog.WarnOnce("BasicTooltipViewModel._tooltipProperties", "BasicTooltipViewModel._tooltipProperties not found; mastery tooltip level inert.");
            return;
        }
        if (TooltipPropertiesField.GetValue(hint) is not Func<List<TooltipProperty>> vanilla || vanilla.Target is MasteryTooltip) return;
        TooltipPropertiesField.SetValue(hint, new Func<List<TooltipProperty>>(new MasteryTooltip(vm, vanilla).Build));
    }

    private List<TooltipProperty> Build()
    {
        List<TooltipProperty> rows = _vanilla();
        PerkObject? perk = _vm.Perk;
        if (rows == null || perk?.AlternativePerk == null) return rows;
        Hero? hero = PerkVMContext.GetHero(_vm);
        if (hero == null || !MasteryRules.IsEnabledFor(hero) || !MasteryRules.IsCandidate(hero, perk)) return rows;
        // GetPerkEffectText (verified): required-level row is last, DefinitionLabel = str_required_level_perk. Matched by label so rows other mods append survive.
        TooltipProperty mastery = new(Texts.MasterySkillLevel.ToString(), MasteryRules.RequiredLevel(perk, hero).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None);
        string requiredLabel = GameTexts.FindText("str_required_level_perk").ToString();
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            if (rows[i].DefinitionLabel != requiredLabel) continue;
            rows[i] = mastery;
            return rows;
        }
        rows.Add(mastery);
        return rows;
    }
}