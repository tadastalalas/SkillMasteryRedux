using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Hero Healing Rate (Intelligence) attribute bonus | gate: setting:HeroHealing | Postfix: add a factor labelled with the attribute from the party leader's Intelligence; prisoners and starving parties are left alone
[HarmonyPatch(typeof(DefaultPartyHealingModel), nameof(DefaultPartyHealingModel.GetDailyHealingHpForHeroes), typeof(PartyBase), typeof(bool), typeof(bool))]
internal static class DefaultPartyHealingModelPatch
{
    private static void Postfix(PartyBase party, bool isPrisoners, ref ExplainedNumber __result)
    {
        if (isPrisoners || party == null || !party.IsMobile || __result.ResultNumber <= 0f) return;
        AttributeBonusRules.AddFactor(ref __result, AttributeBonus.HeroHealing, party.LeaderHero);
    }
}
