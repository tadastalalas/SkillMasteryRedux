using SkillMasteryRedux.Data;

namespace SkillMasteryRedux.Core;

// One attribute bonus as configured. IsEnabled already includes the attribute group toggle.
// Amount: fraction per attribute point; SmithingStamina: stamina per point; CompanionLimit: attribute points per extra companion.
internal readonly struct BonusSetting(bool isEnabled, float amount, BonusRecipients recipients)
{
    internal bool IsEnabled { get; } = isEnabled;

    internal float Amount { get; } = amount;

    internal BonusRecipients Recipients { get; } = recipients;
}
