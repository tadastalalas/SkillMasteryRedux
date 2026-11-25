using HarmonyLib;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Party Leader Experience from Roles (Intelligence) attribute bonus | gate: setting:RoleExperience | Postfix: when a role holder gains experience in the role's skill, give the party leader a share through the leader's own AddSkillXp
[HarmonyPatch(typeof(Hero), nameof(Hero.AddSkillXp), typeof(SkillObject), typeof(float))]
internal static class HeroPatch
{
    private static void Postfix(Hero __instance, SkillObject skill, float xpAmount)
    {
        if (__instance == null || skill == null || xpAmount <= 0f) return;
        MobileParty? party = __instance.PartyBelongedTo;
        Hero? leader = party?.LeaderHero;
        // The leader's own gain re-enters here and stops at this check.
        if (party == null || leader == null || leader == __instance || !IsRoleSkill(party, __instance, skill)) return;
        float share = AttributeBonusRules.Value(AttributeBonus.RoleExperience, leader);
        if (share > 0f) leader.AddSkillXp(skill, xpAmount * share);
    }

    private static bool IsRoleSkill(MobileParty party, Hero hero, SkillObject skill) =>
        (party.EffectiveScout == hero && skill == DefaultSkills.Scouting)
        || (party.EffectiveEngineer == hero && skill == DefaultSkills.Engineering)
        || (party.EffectiveSurgeon == hero && skill == DefaultSkills.Medicine)
        || (party.EffectiveQuartermaster == hero && skill == DefaultSkills.Steward);
}
