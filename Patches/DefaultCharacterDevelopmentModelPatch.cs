using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Minimum learning rate at every skill level | gate: setting:MinimumLearningRate | Postfix: raise vanilla's LimitMin(0) floor on the base method so other CharacterDevelopmentModel overrides that call base keep working; no cap, so skills grow past 330 and every mastery tier stays reachable
[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), nameof(DefaultCharacterDevelopmentModel.CalculateLearningRate), typeof(IReadOnlyPropertyOwner<CharacterAttribute>), typeof(int), typeof(int), typeof(SkillObject), typeof(bool))]
internal static class DefaultCharacterDevelopmentModelPatch
{
    // Clamp only: ExplainedNumber.Add feeds the base that the factors multiply, so a flat line cannot express the floor.
    private static void Postfix(int skillValue, ref ExplainedNumber __result)
    {
        float minimum = MasteryRules.MinimumLearningRate;
        if (minimum <= 0f || __result.ResultNumber >= minimum) return;
        __result.LimitMin(minimum);
    }
}