using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Party Wages (Social) attribute bonus | gate: setting:PartyWages | Postfix: subtract a factor labelled with the attribute from the party leader's Social; a patch instead of a replaced wage model, so other wage models that call base keep it
[HarmonyPatch(typeof(DefaultPartyWageModel), nameof(DefaultPartyWageModel.GetTotalWage), typeof(MobileParty), typeof(TroopRoster), typeof(bool))]
internal static class DefaultPartyWageModelPatch
{
    private static void Postfix(MobileParty mobileParty, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.PartyWages, mobileParty?.LeaderHero, isReduction: true);
}
