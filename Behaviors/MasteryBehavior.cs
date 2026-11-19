using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;

namespace SkillMasteryRedux.Behaviors;

// SMR-FEATURE: Companions and AI lords acquire eligible mastery perks daily | gate: IsAutoMasteryHero
public sealed class MasteryBehavior : CampaignBehaviorBase
{
    public override void RegisterEvents() => CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, OnDailyTickHero);

    // No SyncData keys: nothing is saved, perks live in vanilla hero data.
    public override void SyncData(IDataStore dataStore) { }

    private static void OnDailyTickHero(Hero hero)
    {
        if (hero == null || !MasteryRules.IsAutoMasteryHero(hero)) return;
        MBReadOnlyList<PerkObject> perks = PerkObject.All;
        for (int i = 0; i < perks.Count; i++)
        {
            PerkObject perk = perks[i];
            if (MasteryRules.IsCandidate(hero, perk) && MasteryRules.HasRequiredSkill(hero, perk)) MasteryGrant.TryGrant(hero, perk);
        }
    }
}