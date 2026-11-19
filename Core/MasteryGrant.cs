using SkillMasteryRedux.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace SkillMasteryRedux.Core;

internal static class MasteryGrant
{
    internal static bool TryGrant(Hero hero, PerkObject perk)
    {
        HeroDeveloper? developer = hero.HeroDeveloper;
        if (developer == null)
        {
            SmrLog.WarnOnce("nodeveloper:" + hero.StringId, $"{hero.Name} has no HeroDeveloper; mastery perk {perk.StringId} not granted.");
            return false;
        }
        developer.AddPerk(perk);
        if (MasteryRules.IsPlayer(hero) || MasteryRules.IsPlayerClan(hero)) SmrLog.Debug($"{hero.Name} mastered {perk.StringId} ({perk.Skill.StringId}).");
        return true;
    }
}