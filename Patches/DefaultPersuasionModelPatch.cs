using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.GameComponents;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Persuasion Chance (Social) attribute bonus | gate: setting:PersuasionChance | Postfix: add to vanilla's bonus multiplier, which GetChances applies as success x bonus and critical failure x (2 - bonus), each clamped to 0..1
// String target: called as GetBonusSuccessChance(optionArgs) returning float from GetChances in 1.4.7, 1.4.8 and 1.5.7 (re-verify in dnSpy on game updates).
[HarmonyPatch(typeof(DefaultPersuasionModel), "GetBonusSuccessChance", typeof(PersuasionOptionArgs))]
internal static class DefaultPersuasionModelPatch
{
    private static void Postfix(ref float __result)
    {
        float value = AttributeBonusRules.Value(AttributeBonus.PersuasionChance, Hero.MainHero);
        if (value > 0f) __result += value;
    }
}
