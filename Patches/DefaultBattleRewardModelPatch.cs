using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Renown from Victories (Cunning) attribute bonus | gate: setting:VictoryRenown | Postfix: add a factor labelled with the attribute from the winning party leader's Cunning
[HarmonyPatch(typeof(DefaultBattleRewardModel), nameof(DefaultBattleRewardModel.CalculateRenownGain), typeof(PartyBase), typeof(float), typeof(float), typeof(float), typeof(bool))]
internal static class DefaultBattleRewardModelCalculateRenownGainPatch
{
    private static void Postfix(PartyBase winnerParty, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.VictoryRenown, winnerParty?.LeaderHero);
}

// SMR-FEATURE: Morale from Victories (Cunning) attribute bonus | gate: setting:VictoryMorale | Postfix: add a factor labelled with the attribute from the winning party leader's Cunning
[HarmonyPatch(typeof(DefaultBattleRewardModel), nameof(DefaultBattleRewardModel.CalculateMoraleGainVictory), typeof(PartyBase), typeof(float), typeof(float), typeof(bool))]
internal static class DefaultBattleRewardModelCalculateMoraleGainVictoryPatch
{
    private static void Postfix(PartyBase winnerParty, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.VictoryMorale, winnerParty?.LeaderHero);
}

// SMR-FEATURE: Influence from Victories (Intelligence) attribute bonus | gate: setting:VictoryInfluence | Postfix: add a factor labelled with the attribute from the winning party leader's Intelligence
[HarmonyPatch(typeof(DefaultBattleRewardModel), nameof(DefaultBattleRewardModel.CalculateInfluenceGain), typeof(PartyBase), typeof(float), typeof(float), typeof(float), typeof(bool))]
internal static class DefaultBattleRewardModelCalculateInfluenceGainPatch
{
    private static void Postfix(PartyBase winnerParty, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.VictoryInfluence, winnerParty?.LeaderHero);
}
