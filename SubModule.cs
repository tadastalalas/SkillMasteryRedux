using System;
using HarmonyLib;
using Bannerlord.UIExtenderEx;
using SkillMasteryRedux.Behaviors;
using SkillMasteryRedux.Compat;
using SkillMasteryRedux.Core;
using SkillMasteryRedux.Diagnostics;
using SkillMasteryRedux.UI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SkillMasteryRedux;

public class SubModule : MBSubModuleBase
{
    private const string LegacyHarmonyId = "com.skillmastery.perkunlocker";
    private const string LegacyFixHarmonyId = "com.skillmasteryfix";

    private const string ShowSkillLimitHarmonyId = "ShowSkillLimit";
    private const string MinimumLearningRateModelType = "MinimumLearningRate.MinimumLearningRateModel";
    private const string StaticInitializerFrame = ".cctor";

    private Harmony? _harmony;
    private UIExtender? _uiExtender;
    [ThreadStatic] private static bool _isReportingFirstChance;

    // PatchAll runs here: attribute patches are active from module load on.
    protected override void OnSubModuleLoad()
    {
        base.OnSubModuleLoad();
        AppDomain.CurrentDomain.FirstChanceException += ReportStaticInitializerFailure;
        try
        {
            _harmony = new Harmony(ModInfo.HarmonyId);
            _harmony.PatchAll(typeof(SubModule).Assembly);
            _uiExtender = UIExtender.Create(ModInfo.UIExtenderId);
            _uiExtender.Register(typeof(SubModule).Assembly);
            _uiExtender.Enable();
            SmrLog.Debug("Patches and UI extensions applied.");
        }
        catch (Exception ex)
        {
            SmrLog.Error("PatchAll failed.", ex);
            throw;
        }
        finally
        {
            AppDomain.CurrentDomain.FirstChanceException -= ReportStaticInitializerFailure;
        }
    }

    // Compiling a patched method can run the static initializer of a class it reads a static field from, before any game exists;
    // a failure there breaks that class for the whole session, so it is reported while the patches are applied.
    private static void ReportStaticInitializerFailure(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
        if (_isReportingFirstChance) return;
        _isReportingFirstChance = true;
        try
        {
            string stack = Environment.StackTrace;
            if (stack.Contains(StaticInitializerFrame))
                SmrLog.Error("A static initializer failed while patches were applied. Stack:" + Environment.NewLine + stack, e.Exception);
        }
        finally
        {
            _isReportingFirstChance = false;
        }
    }

    protected override void OnSubModuleUnloaded()
    {
        base.OnSubModuleUnloaded();
        _harmony?.UnpatchAll(ModInfo.HarmonyId);
    }

    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        base.OnBeforeInitialModuleScreenSetAsRoot();
        if (Harmony.HasAnyPatches(LegacyHarmonyId) || Harmony.HasAnyPatches(LegacyFixHarmonyId))
            SmrLog.Warn("SkillMastery / SkillMasteryFix detected. Disable them; SkillMasteryRedux replaces both.");
        if (Harmony.HasAnyPatches(ShowSkillLimitHarmonyId))
            SmrLog.Warn("Show Skill Limit detected. Disable it; SkillMasteryRedux includes the skill limit marker.");
        if (AccessTools.TypeByName(MinimumLearningRateModelType) != null)
            SmrLog.Warn("Minimum Learning Rate detected. Disable it; SkillMasteryRedux includes a configurable minimum learning rate.");
        // Runs once: every module's patches exist by now.
        if (_harmony != null) BetterAttributesCompat.TryApply(_harmony);
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStart(game, gameStarterObject);
        ModSettings.Reload();
        MasteryRules.ResetCache();
        if (gameStarterObject is CampaignGameStarter campaignStarter) campaignStarter.AddBehavior(new MasteryBehavior());
    }

    public override void OnGameEnd(Game game)
    {
        base.OnGameEnd(game);
        MasteryRules.ResetCache();
        SkillLimitBar.Reset();
    }
}