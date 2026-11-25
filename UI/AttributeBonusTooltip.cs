using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;

namespace SkillMasteryRedux.UI;

// SMR-FEATURE: Attribute tooltip lists the attribute's bonuses at its current value | gate: setting:EnableAttributeBonuses
internal static class AttributeBonusTooltip
{
    private const char LineBreak = '\n';
    private const string ListSeparator = "\n\n";
    private const string NumberFormat = "0.#";
    private const string Plus = "+";
    private const string Minus = "-";
    private const float PercentScale = 100f;

    // Per attribute row: vanilla's increase hint and the text this class wrote last, so every rebuild starts from vanilla's text.
    private static readonly ConditionalWeakTable<CharacterAttributeItemVM, HintState> Hints = new();

    // Constructor and RefreshValues. The hint counts as vanilla's only when it differs from what was written last:
    // the constructor postfix runs after the patched RefreshValues already added the list.
    internal static void OnRefreshValues(CharacterAttributeItemVM row, Hero? hero)
    {
        string current = row.IncreaseHelpText ?? string.Empty;
        if (!Hints.TryGetValue(row, out HintState state))
        {
            state = new HintState { Vanilla = current };
            Hints.Add(row, state);
        }
        else if (current != state.Written)
        {
            state.Vanilla = current;
        }

        Apply(row, hero, state);
    }

    // RefreshWithCurrentValues changes AttributeValue without touching the hint. The constructor calls it before
    // RefreshValues; nothing is stored yet then, and the list is built right after.
    internal static void OnRefreshWithCurrentValues(CharacterAttributeItemVM row, Hero? hero)
    {
        if (Hints.TryGetValue(row, out HintState state)) Apply(row, hero, state);
    }

    private static void Apply(CharacterAttributeItemVM row, Hero? hero, HintState state)
    {
        string vanilla = state.Vanilla;
        CharacterAttribute? attribute = row.AttributeType;
        string bonuses = hero == null || attribute == null ? string.Empty : BuildList(hero, attribute, row.AttributeValue);
        string text = bonuses.Length == 0 ? vanilla
            : AttributeBonusRules.ShowOnlyBonusesInAttributeTooltip || vanilla.Length == 0 ? bonuses
            : bonuses + ListSeparator + vanilla;
        state.Written = text;
        if (row.IncreaseHelpText != text) row.IncreaseHelpText = text;
    }

    private sealed class HintState
    {
        internal string Vanilla { get; set; } = string.Empty;

        internal string Written { get; set; } = string.Empty;
    }

    private static string BuildList(Hero hero, CharacterAttribute attribute, int points)
    {
        if (!AttributeBonusRules.IsEnabled) return string.Empty;
        StringBuilder list = new();
        for (int i = 0; i < AttributeBonusRules.BonusCount; i++)
        {
            AttributeBonus bonus = (AttributeBonus)i;
            if (AttributeBonusRules.AttributeOf(bonus) != attribute) continue;
            float value = AttributeBonusRules.Value(bonus, hero, points);
            if (value <= 0f) continue;
            if (list.Length > 0) list.Append(LineBreak);
            list.Append(Texts.AttributeBonusLine(bonus, Format(bonus, value)));
        }

        return list.ToString();
    }

    private static string Format(AttributeBonus bonus, float value) => bonus switch
    {
        AttributeBonus.SmithingStamina or AttributeBonus.CompanionLimit => Plus + Number(value),
        AttributeBonus.PartyWages => Texts.Percent(Minus + Number(value * PercentScale)),
        // A chance and a share, not an increase.
        AttributeBonus.SliceThrough or AttributeBonus.RoleExperience => Texts.Percent(Number(value * PercentScale)),
        _ => Texts.Percent(Plus + Number(value * PercentScale)),
    };

    private static string Number(float value) => value.ToString(NumberFormat, CultureInfo.InvariantCulture);
}
