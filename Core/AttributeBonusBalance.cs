namespace SkillMasteryRedux.Core;

// Vanilla limits the attribute bonuses keep.
internal static class AttributeBonusBalance
{
    // MissionCombatMechanicsHelper.ComputeBlowMagnitude clamps specialMagnitude to 0..500.
    internal const float MaxBlowMagnitude = 500f;
}
