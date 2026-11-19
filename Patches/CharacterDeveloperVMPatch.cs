using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using SkillMasteryRedux.UI;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Hero dropdown marks unspent attribute/focus points and unopened perks | gate: setting:ShowHeroListMarkers | Postfix: vanilla rebuilds the selector from names only; swap in rows that carry the markers without re-firing the selection
[HarmonyPatch(typeof(CharacterDeveloperVM), "RefreshCharacterSelector")]
internal static class CharacterDeveloperVMPatch
{
    // Fragile: private vanilla name, re-verify in dnSpy on game updates.
    private static readonly FieldInfo? OnChangeField = AccessTools.Field(typeof(SelectorVM<SelectorItemVM>), "_onChange");

    private static readonly Action<SelectorVM<SelectorItemVM>> NoChange = _ => { };

    // Rows of the previous rebuild per screen, unhooked from the hero/skill VMs before they are replaced.
    private static readonly ConditionalWeakTable<CharacterDeveloperVM, List<HeroSelectorItemVM>> Rows = new();

    // ____heroList / ____heroIndex: vanilla private fields _heroList / _heroIndex (re-verify in dnSpy on game updates).
    private static void Postfix(CharacterDeveloperVM __instance, List<CharacterDeveloperHeroItemVM> ____heroList, int ____heroIndex)
    {
        if (__instance == null || ____heroList == null) return;
        SelectorVM<SelectorItemVM>? selector = __instance.CharacterList;
        if (selector == null) return;
        if (OnChangeField == null)
        {
            SmrLog.WarnOnce("SelectorVM._onChange", "SelectorVM._onChange not found; hero dropdown markers disabled.");
            return;
        }
        Action<SelectorVM<SelectorItemVM>>? onChange = OnChangeField.GetValue(selector) as Action<SelectorVM<SelectorItemVM>>;

        List<HeroSelectorItemVM> rows = Rows.GetValue(__instance, _ => []);
        for (int i = 0; i < rows.Count; i++) rows[i].Unhook();
        rows.Clear();

        bool isTracking = MasteryRules.ShowHeroListMarkers;
        for (int i = 0; i < ____heroList.Count; i++)
        {
            CharacterDeveloperHeroItemVM hero = ____heroList[i];
            if (hero == null) continue;
            rows.Add(new HeroSelectorItemVM(hero, isTracking));
        }

        // Clearing ItemList makes the bound dropdown push index 0 into SelectedIndex; with vanilla's action attached that would select the main hero.
        // Vanilla's Refresh already ran OnCharacterSelection, so the rows go in with a no-op action, then the original action is restored.
        selector.SetOnChangeAction(NoChange);
        selector.Refresh(rows, ____heroIndex, NoChange);
        selector.SetOnChangeAction(onChange);
    }
}