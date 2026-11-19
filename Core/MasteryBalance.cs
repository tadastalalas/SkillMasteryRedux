namespace SkillMasteryRedux.Core;

// Vanilla constants the mastery formula is built on.
internal static class MasteryBalance
{
    // Spacing between vanilla perk tiers (25, 50, ... 275); tier index = RequiredSkillValue / TierStep.
    internal const float TierStep = 25f;

    // Vanilla's natural skill cap (learning rate reaches zero at 330 with max attributes and focus); with safeguards on,
    // the last mastery tier never exceeds it. Skills themselves may grow past it under the minimum learning rate.
    internal const int SkillCap = 330;
}