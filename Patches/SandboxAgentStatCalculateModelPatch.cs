using System.Runtime.CompilerServices;
using HarmonyLib;
using SandBox.GameComponents;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Draw Speed (Vigor), Accuracy, Reload Speed, Weapon Handling (Control), Aim Stability and Movement Speed (Endurance) attribute bonuses | gate: setting:<each bonus> | Postfix: vanilla recalculates the hero's driven properties here; scale them afterwards
[HarmonyPatch(typeof(SandboxAgentStatCalculateModel), nameof(SandboxAgentStatCalculateModel.UpdateAgentStats), typeof(Agent), typeof(AgentDrivenProperties))]
internal static class SandboxAgentStatCalculateModelPatch
{
    private const int DrawSpeedSlot = 0;
    private const int AccuracySlot = 1;
    private const int ReloadSpeedSlot = 2;
    private const int WeaponHandlingSlot = 3;
    private const int AimStabilitySlot = 4;
    private const int MovementSpeedSlot = 5;
    private const int SlotCount = 6;

    // Value written by this patch per property. Vanilla does not rewrite every property on every call (weapon stats only for
    // the wielded weapon), so a value still equal to what was written last time already carries the bonus and is left alone.
    private static readonly ConditionalWeakTable<AgentDrivenProperties, float[]> Written = new();

    private static void Postfix(Agent agent, AgentDrivenProperties agentDrivenProperties)
    {
        if (agent == null || agentDrivenProperties == null || !agent.IsHuman || !AttributeBonusRules.IsEnabled) return;
        Hero? hero = AttributeBonusRules.HeroOf(agent);
        if (hero == null) return;
        float[] written = Written.GetValue(agentDrivenProperties, _ => new float[SlotCount]);
        AgentDrivenProperties p = agentDrivenProperties;

        float value = AttributeBonusRules.Value(AttributeBonus.DrawSpeed, hero);
        if (value > 0f && IsRewritten(p.ThrustOrRangedReadySpeedMultiplier, written, DrawSpeedSlot))
            p.ThrustOrRangedReadySpeedMultiplier = written[DrawSpeedSlot] = p.ThrustOrRangedReadySpeedMultiplier * (1f + value);

        value = AttributeBonusRules.Value(AttributeBonus.Accuracy, hero);
        if (value > 0f && IsRewritten(p.WeaponInaccuracy, written, AccuracySlot))
            p.WeaponInaccuracy = written[AccuracySlot] = p.WeaponInaccuracy / (1f + value);

        value = AttributeBonusRules.Value(AttributeBonus.ReloadSpeed, hero);
        if (value > 0f && IsRewritten(p.ReloadSpeed, written, ReloadSpeedSlot))
            p.ReloadSpeed = written[ReloadSpeedSlot] = p.ReloadSpeed * (1f + value);

        value = AttributeBonusRules.Value(AttributeBonus.WeaponHandling, hero);
        if (value > 0f && IsRewritten(p.HandlingMultiplier, written, WeaponHandlingSlot))
            p.HandlingMultiplier = written[WeaponHandlingSlot] = p.HandlingMultiplier * (1f + value);

        // The shaking starts later and reaches full strength later by the same time, so vanilla's ramp keeps its length.
        value = AttributeBonusRules.Value(AttributeBonus.AimStability, hero);
        if (value > 0f && IsRewritten(p.WeaponUnsteadyBeginTime, written, AimStabilitySlot))
        {
            float delay = p.WeaponUnsteadyBeginTime * value;
            p.WeaponUnsteadyBeginTime = written[AimStabilitySlot] = p.WeaponUnsteadyBeginTime + delay;
            p.WeaponUnsteadyEndTime += delay;
        }

        value = AttributeBonusRules.Value(AttributeBonus.MovementSpeed, hero);
        if (value > 0f && IsRewritten(p.MaxSpeedMultiplier, written, MovementSpeedSlot))
            p.MaxSpeedMultiplier = written[MovementSpeedSlot] = p.MaxSpeedMultiplier * (1f + value);
    }

    private static bool IsRewritten(float current, float[] written, int slot) => current != written[slot];
}
