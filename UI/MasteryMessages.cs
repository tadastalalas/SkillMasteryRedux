using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SkillMasteryRedux.UI;

// SMR-FEATURE: Mastery feedback messages | gate: setting:ShowMessages
internal static class MasteryMessages
{
    internal static void OnMastered(Hero hero, PerkObject perk)
    {
        if (!MasteryRules.ShowMessages) return;
        bool final = MasteryRules.IsFinalTier(perk);
        TextObject text = MasteryRules.IsPlayer(hero)
            ? (final ? Texts.PlayerMastered(perk.Skill) : Texts.PlayerMasteryGrows())
            : (final ? Texts.HeroMastered(hero.Name, perk.Skill) : Texts.HeroMasteryGrows(hero.Name));
        InformationManager.DisplayMessage(new InformationMessage(text.ToString()));
    }
}