using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Melee Damage (Vigor) and Ranged Damage (Control) attribute bonuses | gate: setting:MeleeDamage / setting:RangedDamage | Postfix: scale the blow before armor like vanilla's weapon damage multiplier does, then keep vanilla's magnitude cap
// String target: private method, same signature in 1.4.7, 1.4.8 and 1.5.7 (re-verify in dnSpy on game updates).
[HarmonyPatch(typeof(MissionCombatMechanicsHelper), "ComputeBlowMagnitude",
    new[] { typeof(AttackCollisionData), typeof(AttackInformation), typeof(float), typeof(bool), typeof(bool), typeof(Vec2), typeof(Vec2), typeof(float), typeof(float), typeof(float), typeof(int) },
    new[] { ArgumentType.Ref, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out, ArgumentType.Out, ArgumentType.Out })]
internal static class MissionCombatMechanicsHelperComputeBlowMagnitudePatch
{
    private static void Postfix(in AttackCollisionData acd, in AttackInformation attackInformation, ref float baseMagnitude, ref float specialMagnitude)
    {
        // Weapon swings, thrusts and missiles only: no falls, horse charges, kicks, bashes or unarmed hits.
        if (acd.IsFallDamage || acd.IsHorseCharge || acd.IsAlternativeAttack) return;
        if (!acd.IsMissile && attackInformation.AttackerWeapon.IsEmpty) return;
        AttributeBonus bonus = acd.IsMissile ? AttributeBonus.RangedDamage : AttributeBonus.MeleeDamage;
        float value = AttributeBonusRules.Value(bonus, AttributeBonusRules.HeroOf(attackInformation.AttackerAgentCharacter));
        if (value <= 0f) return;
        float factor = 1f + value;
        baseMagnitude *= factor;
        specialMagnitude = MBMath.ClampFloat(specialMagnitude * factor, 0f, AttributeBonusBalance.MaxBlowMagnitude);
    }
}

// SMR-FEATURE: Slice Through Chance (Vigor) attribute bonus | gate: setting:SliceThrough | Prefix: skipping vanilla leaves momentumRemaining unchanged, so the swing keeps its full power after cutting through
[HarmonyPatch(typeof(MissionCombatMechanicsHelper), nameof(MissionCombatMechanicsHelper.UpdateMomentumRemaining),
    new[] { typeof(float), typeof(Blow), typeof(AttackCollisionData), typeof(Agent), typeof(Agent), typeof(MissionWeapon), typeof(bool) },
    new[] { ArgumentType.Ref, ArgumentType.Ref, ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
internal static class MissionCombatMechanicsHelperUpdateMomentumRemainingPatch
{
    private static bool Prefix(in AttackCollisionData collisionData, Agent attacker, in MissionWeapon attackerWeapon, bool isCrushThrough)
    {
        // Melee weapon swings and thrusts that cut through only: vanilla's crush-through momentum loss stays.
        if (isCrushThrough || collisionData.IsMissile || collisionData.IsAlternativeAttack || attackerWeapon.IsEmpty) return true;
        float chance = AttributeBonusRules.Value(AttributeBonus.SliceThrough, AttributeBonusRules.HeroOf(attacker));
        return chance <= 0f || MBRandom.RandomFloat >= chance;
    }
}
