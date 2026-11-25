using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Max Hit Points (Endurance) attribute bonus | gate: setting:MaxHitPoints | Postfix: add a factor labelled with the attribute to vanilla's hit point breakdown
[HarmonyPatch(typeof(DefaultCharacterStatsModel), nameof(DefaultCharacterStatsModel.MaxHitpoints), typeof(CharacterObject), typeof(bool))]
internal static class DefaultCharacterStatsModelPatch
{
    private static void Postfix(CharacterObject character, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.MaxHitPoints, AttributeBonusRules.HeroOf(character));
}
