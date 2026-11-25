using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Party Morale (Social) attribute bonus | gate: setting:PartyMorale | Postfix: add a factor labelled with the attribute from the party leader's Social
[HarmonyPatch(typeof(DefaultPartyMoraleModel), nameof(DefaultPartyMoraleModel.GetEffectivePartyMorale), typeof(MobileParty), typeof(bool))]
internal static class DefaultPartyMoraleModelPatch
{
    private static void Postfix(MobileParty mobileParty, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.PartyMorale, mobileParty?.LeaderHero);
}
