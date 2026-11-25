using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Party Size (Social) attribute bonus | gate: setting:PartySize | Postfix: add a factor labelled with the attribute from the party leader's Social
#if LOWER_THAN_1_5
// 1.4.7, 1.4.8
[HarmonyPatch(typeof(DefaultPartySizeLimitModel), nameof(DefaultPartySizeLimitModel.GetPartyMemberSizeLimit), typeof(PartyBase), typeof(bool))]
#else
// 1.5.7: bool isCurrentlyAtSea parameter added before includeDescriptions.
[HarmonyPatch(typeof(DefaultPartySizeLimitModel), nameof(DefaultPartySizeLimitModel.GetPartyMemberSizeLimit), typeof(PartyBase), typeof(bool), typeof(bool))]
#endif
internal static class DefaultPartySizeLimitModelPatch
{
    private static void Postfix(PartyBase party, ref ExplainedNumber __result) =>
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.PartySize, party?.LeaderHero);
}
