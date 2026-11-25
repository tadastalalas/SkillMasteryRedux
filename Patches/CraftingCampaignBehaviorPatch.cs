using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Library;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Smithing Stamina (Endurance) attribute bonus | gate: setting:SmithingStamina | Postfix: add stamina per point to vanilla's maximum (100 + Smithing / 2)
[HarmonyPatch(typeof(CraftingCampaignBehavior), nameof(CraftingCampaignBehavior.GetMaxHeroCraftingStamina), typeof(Hero))]
internal static class CraftingCampaignBehaviorPatch
{
    private static void Postfix(Hero hero, ref int __result)
    {
        float value = AttributeBonusRules.Value(AttributeBonus.SmithingStamina, hero);
        if (value > 0f) __result += MathF.Round(value);
    }
}
