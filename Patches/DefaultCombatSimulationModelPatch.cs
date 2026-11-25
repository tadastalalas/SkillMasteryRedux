using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
#if !LOWER_THAN_1_5
using TaleWorlds.Core;
#endif

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Simulation Advantage (Cunning) attribute bonus | gate: setting:SimulationAdvantage | Postfix: every simulated hit of the party is scaled by its leader's Cunning, like a party-wide perk
// Troop overload only: vanilla's ship overload always returns 0 in every supported version.
#if LOWER_THAN_1_5
// 1.4.7, 1.4.8
[HarmonyPatch(typeof(DefaultCombatSimulationModel), nameof(DefaultCombatSimulationModel.SimulateHit),
    typeof(CharacterObject), typeof(CharacterObject), typeof(PartyBase), typeof(PartyBase), typeof(float), typeof(MapEvent), typeof(float), typeof(float))]
#else
// 1.5.7: BattleEnvironment parameter added after the MapEvent.
[HarmonyPatch(typeof(DefaultCombatSimulationModel), nameof(DefaultCombatSimulationModel.SimulateHit),
    typeof(CharacterObject), typeof(CharacterObject), typeof(PartyBase), typeof(PartyBase), typeof(float), typeof(MapEvent), typeof(BattleEnvironment), typeof(float), typeof(float))]
#endif
internal static class DefaultCombatSimulationModelPatch
{
    private static void Postfix(PartyBase strikerParty, ref ExplainedNumber __result)
    {
        float value = AttributeBonusRules.Value(AttributeBonus.SimulationAdvantage, strikerParty?.LeaderHero);
        if (value > 0f) __result.AddFactor(value);
    }
}
