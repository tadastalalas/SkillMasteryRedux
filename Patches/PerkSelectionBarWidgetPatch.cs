using HarmonyLib;
using SkillMasteryRedux.UI;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper;

namespace SkillMasteryRedux.Patches;

// SMR-FEATURE: Skill limit marker on the character screen skill bar | gate: setting:ShowSkillLimit | Postfix: vanilla lays the bar out here every frame; size the limit segment after it
[HarmonyPatch(typeof(PerkSelectionBarWidget), "OnLateUpdate")]
internal static class PerkSelectionBarWidgetPatch
{
    private static void Postfix(PerkSelectionBarWidget __instance)
    {
        if (__instance == null) return;
        SkillLimitBar.Update(__instance);
    }
}