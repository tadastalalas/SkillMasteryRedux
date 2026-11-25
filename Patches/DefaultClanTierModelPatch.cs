using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Companion Limit (Social) attribute bonus | gate: setting:CompanionLimit | Postfix: add companions from each clan's own leader's Social
[HarmonyPatch(typeof(DefaultClanTierModel), nameof(DefaultClanTierModel.GetCompanionLimit), typeof(Clan))]
internal static class DefaultClanTierModelPatch
{
    private static void Postfix(Clan clan, ref int __result)
    {
        int extra = (int)AttributeBonusRules.Value(AttributeBonus.CompanionLimit, clan?.Leader);
        if (extra > 0) __result += extra;
    }
}
