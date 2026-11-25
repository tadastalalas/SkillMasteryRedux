using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Clan Income (Intelligence) attribute bonus | gate: setting:ClanIncome | Postfix: add a share of the income collected so far, labelled with the attribute, from the clan leader's Intelligence
// String target: private method, same signature in 1.4.7, 1.4.8 and 1.5.7 (re-verify in dnSpy on game updates).
[HarmonyPatch(typeof(DefaultClanFinanceModel), "CalculateClanIncomeInternal",
    new[] { typeof(Clan), typeof(ExplainedNumber), typeof(bool), typeof(bool) },
    new[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal })]
internal static class DefaultClanFinanceModelPatch
{
    private static void Postfix(Clan clan, ref ExplainedNumber goldChange)
    {
        if (clan == null || clan.IsEliminated) return;
        float value = AttributeBonusRules.Value(AttributeBonus.ClanIncome, clan.Leader);
        float income = goldChange.ResultNumber;
        if (value <= 0f || income <= 0f) return;
        goldChange.Add(income * value, AttributeBonusRules.AttributeOf(AttributeBonus.ClanIncome).Name);
    }
}
