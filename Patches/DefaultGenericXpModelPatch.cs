using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Experience Gain (Intelligence) attribute bonus | gate: setting:ExperienceGain | Postfix: scale vanilla's experience multiplier by the hero's Intelligence
[HarmonyPatch(typeof(DefaultGenericXpModel), nameof(DefaultGenericXpModel.GetXpMultiplier), typeof(Hero))]
internal static class DefaultGenericXpModelPatch
{
    private static void Postfix(Hero hero, ref float __result)
    {
        float value = AttributeBonusRules.Value(AttributeBonus.ExperienceGain, hero);
        if (value > 0f) __result *= 1f + value;
    }
}
