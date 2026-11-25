using HarmonyLib;
using SandBox.GameComponents;
using SkillMasteryRedux.Core;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Stagger Resistance (Endurance) attribute bonus | gate: setting:StaggerResistance | Postfix: raise the damage the hero takes without the attack in progress being interrupted
[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), nameof(SandboxAgentApplyDamageModel.CalculateStaggerThresholdDamage),
    new[] { typeof(Agent), typeof(Blow) },
    new[] { ArgumentType.Normal, ArgumentType.Ref })]
internal static class SandboxAgentApplyDamageModelCalculateStaggerThresholdDamagePatch
{
    private static void Postfix(Agent defenderAgent, ref float __result)
    {
        float value = AttributeBonusRules.Value(AttributeBonus.StaggerResistance, AttributeBonusRules.HeroOf(defenderAgent));
        if (value > 0f) __result *= 1f + value;
    }
}

// SMR-FEATURE: Crush Through Power (Vigor) attribute bonus | gate: setting:CrushThrough | Prefix: raise the attack energy before vanilla's own check, so vanilla still decides (overhead swings only)
[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), nameof(SandboxAgentApplyDamageModel.DecideCrushedThrough),
    typeof(Agent), typeof(Agent), typeof(float), typeof(Agent.UsageDirection), typeof(StrikeType), typeof(WeaponComponentData), typeof(bool))]
internal static class SandboxAgentApplyDamageModelDecideCrushedThroughPatch
{
    private static void Prefix(Agent attackerAgent, ref float totalAttackEnergy)
    {
        float value = AttributeBonusRules.Value(AttributeBonus.CrushThrough, AttributeBonusRules.HeroOf(attackerAgent));
        if (value > 0f) totalAttackEnergy *= 1f + value;
    }
}
