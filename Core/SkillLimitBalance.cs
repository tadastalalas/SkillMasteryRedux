namespace SkillMasteryRedux.Core;

// Derived from DefaultCharacterDevelopmentModel.CalculateLearningRate: past the learning limit the rate is
// 1.25 × (0.4·attributeAverage + focus − 0.1·levelsOverLimit), so it reaches zero 0.4/0.1 = 4 levels per attribute point
// and 1/0.1 = 10 levels per focus point above the rounded limit. Re-verify the 0.4 / 1 / 0.1 factors in dnSpy on game updates.
internal static class SkillLimitBalance
{
    internal const int LevelsOverLimitPerAttributePoint = 4;
    internal const int LevelsOverLimitPerFocusPoint = 10;
}