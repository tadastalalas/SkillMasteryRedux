using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SkillMasteryRedux.Data;
using SkillMasteryRedux.Diagnostics;

namespace SkillMasteryRedux.Core;

// The only place mod code reads settings from. MCM loaded -> MCM. MCM not loaded -> ModConfiguration.xml.
internal static class ModSettings
{
    private const string McmAssemblyName = "MCMv5";

    // Default values for MCM reads while MCM has no settings instance.
    private static readonly ModConfiguration Defaults = new();

    private static ModConfiguration _xml = new();
    private static XmlSerializer? _serializer;
    private static bool _useMcm;

    internal static bool ShowMessages => _useMcm ? McmShowMessages() : _xml.GlobalSettings.ShowMessages;

    internal static bool DisableSafeguards => _useMcm ? McmDisableSafeguards() : _xml.GlobalSettings.DisableSafeguards;

    internal static bool ShowSkillLimit => _useMcm ? McmShowSkillLimit() : _xml.GlobalSettings.ShowSkillLimit;

    internal static bool ShowHeroListMarkers => _useMcm ? McmShowHeroListMarkers() : _xml.GlobalSettings.ShowHeroListMarkers;

    internal static int PlayerMasteryLevelOffset => _useMcm ? McmPlayerMasteryLevelOffset() : _xml.PlayerSettings.PlayerMasteryLevelOffset;

    internal static int PlayerMasteryStartingLevelOffset => _useMcm ? McmPlayerMasteryStartingLevelOffset() : _xml.PlayerSettings.PlayerMasteryStartingLevelOffset;

    internal static bool AutoAssignPlayer => _useMcm ? McmAutoAssignPlayer() : _xml.PlayerSettings.AutoAssignPlayer;

    internal static bool AllowCompanions => _useMcm ? McmAllowCompanions() : _xml.CompanionSettings.AllowCompanions;

    internal static int CompanionMasteryLevelOffset => _useMcm ? McmCompanionMasteryLevelOffset() : _xml.CompanionSettings.CompanionMasteryLevelOffset;

    internal static int CompanionMasteryStartingLevelOffset => _useMcm ? McmCompanionMasteryStartingLevelOffset() : _xml.CompanionSettings.CompanionMasteryStartingLevelOffset;

    internal static bool AutoAssignCompanions => _useMcm ? McmAutoAssignCompanions() : _xml.CompanionSettings.AutoAssignCompanions;

    internal static bool AllowAILords => _useMcm ? McmAllowAILords() : _xml.AILordSettings.AllowAILords;

    internal static float MinimumLearningRate => _useMcm ? McmMinimumLearningRate() : _xml.LearningSettings.MinimumLearningRate;

    internal static bool AttributeBonusesEnabled => _useMcm ? McmAttributeBonusesEnabled() : _xml.AttributeBonusSettings.EnableAttributeBonuses;

    internal static bool OverwriteBetterAttributes => _useMcm ? McmOverwriteBetterAttributes() : _xml.AttributeBonusSettings.OverwriteBetterAttributes;

    internal static bool ShowOnlyBonusesInAttributeTooltip => _useMcm ? McmShowOnlyBonusesInAttributeTooltip() : _xml.AttributeBonusSettings.CharacterScreen.ShowOnlyBonusesInAttributeTooltip;

    internal static BonusSetting Bonus(AttributeBonus bonus) => _useMcm ? McmBonus(bonus) : _xml.AttributeBonusSettings.GetBonus(bonus);

    internal static bool LoggingEnabled => _useMcm ? McmLoggingEnabled() : _xml.TechnicalSettings.LoggingEnabled;

    private static XmlSerializer Serializer => _serializer ??= new XmlSerializer(typeof(ModConfiguration));

    // Called on every campaign start.
    internal static void Reload()
    {
        _useMcm = false;
        _xml = new ModConfiguration();

        if (IsMcmLoaded())
        {
            if (McmHasSettings())
            {
                _useMcm = true;
                return;
            }

            SmrLog.Warn("MCM is loaded but returned no settings for this mod. Using " + ModInfo.ConfigurationFileName + " instead.");
        }

        _xml = LoadFromFile();
    }

    private static bool IsMcmLoaded()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            if (assemblies[i].GetName().Name == McmAssemblyName) return true;
        }

        return false;
    }

    // Every method that touches MCMSettings must be NoInlining and may only be called when MCM is loaded.
    // Otherwise the game tries to load MCM's classes and crashes when MCM is not installed.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmHasSettings() => MCMSettings.Instance != null;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmShowMessages() => MCMSettings.Instance?.ShowMessages ?? ModConfiguration.ShowMessagesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmDisableSafeguards() => MCMSettings.Instance?.DisableSafeguards ?? ModConfiguration.DisableSafeguardsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmShowSkillLimit() => MCMSettings.Instance?.ShowSkillLimit ?? ModConfiguration.ShowSkillLimitDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmShowHeroListMarkers() => MCMSettings.Instance?.ShowHeroListMarkers ?? ModConfiguration.ShowHeroListMarkersDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmPlayerMasteryLevelOffset() => MCMSettings.Instance?.PlayerMasteryLevelOffset ?? ModConfiguration.MasteryLevelOffsetDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmPlayerMasteryStartingLevelOffset() => MCMSettings.Instance?.PlayerMasteryStartingLevelOffset ?? ModConfiguration.MasteryStartingLevelOffsetDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmAutoAssignPlayer() => MCMSettings.Instance?.AutoAssignPlayer ?? ModConfiguration.AutoAssignPlayerDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmAllowCompanions() => MCMSettings.Instance?.AllowCompanions ?? ModConfiguration.AllowCompanionsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmCompanionMasteryLevelOffset() => MCMSettings.Instance?.CompanionMasteryLevelOffset ?? ModConfiguration.MasteryLevelOffsetDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmCompanionMasteryStartingLevelOffset() => MCMSettings.Instance?.CompanionMasteryStartingLevelOffset ?? ModConfiguration.MasteryStartingLevelOffsetDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmAutoAssignCompanions() => MCMSettings.Instance?.AutoAssignCompanions ?? ModConfiguration.AutoAssignCompanionsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmAllowAILords() => MCMSettings.Instance?.AllowAILords ?? ModConfiguration.AllowAILordsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float McmMinimumLearningRate() => MCMSettings.Instance?.MinimumLearningRate ?? ModConfiguration.MinimumLearningRateDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmAttributeBonusesEnabled() => MCMSettings.Instance?.EnableAttributeBonuses ?? ModConfiguration.EnableAttributeBonusesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmOverwriteBetterAttributes() => MCMSettings.Instance?.OverwriteBetterAttributes ?? ModConfiguration.OverwriteBetterAttributesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmShowOnlyBonusesInAttributeTooltip() => MCMSettings.Instance?.ShowOnlyBonusesInAttributeTooltip ?? ModConfiguration.ShowOnlyBonusesInAttributeTooltipDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static BonusSetting McmBonus(AttributeBonus bonus) => MCMSettings.Instance?.GetBonus(bonus) ?? Defaults.AttributeBonusSettings.GetBonus(bonus);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmLoggingEnabled() => MCMSettings.Instance?.LoggingEnabled ?? ModConfiguration.LoggingEnabledDefault;

    private static ModConfiguration LoadFromFile()
    {
        string path = Path.Combine(ModInfo.ModuleRoot, ModInfo.ConfigurationFileName);
        ModConfiguration configuration = new();

        if (!File.Exists(path))
        {
            SmrLog.Warn(ModInfo.ConfigurationFileName + " was missing. Created it with the default settings.");
            TryWrite(path, configuration);
            return configuration;
        }

        try
        {
            configuration = Read(path);
            if (!HasEverySetting(path, configuration))
            {
                SmrLog.Warn(ModInfo.ConfigurationFileName + " had missing settings. Added them with their default values.");
                TryWrite(path, configuration);
            }
        }
        catch (Exception ex) when (ex is InvalidOperationException or XmlException)
        {
            SmrLog.Warn(ModInfo.ConfigurationFileName + " was broken and was replaced with the default settings. Reason: " + (ex.InnerException?.Message ?? ex.Message));
            configuration = new ModConfiguration();
            TryWrite(path, configuration);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            SmrLog.Warn(ModInfo.ConfigurationFileName + " could not be opened. Using the default settings. Reason: " + ex.Message);
            configuration = new ModConfiguration();
        }

        // Out-of-range values are corrected in memory only; the file keeps what the player typed.
        configuration.Clamp();
        return configuration;
    }

    private static ModConfiguration Read(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Serializer.Deserialize(stream) as ModConfiguration ?? new ModConfiguration();
    }

    private static void TryWrite(string path, ModConfiguration configuration)
    {
        try
        {
            XmlWriterSettings writerSettings = new() { Indent = true, Encoding = Encoding.UTF8 };
            using XmlWriter writer = XmlWriter.Create(path, writerSettings);
            Serializer.Serialize(writer, configuration, NoNamespaces());
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            SmrLog.Warn(ModInfo.ConfigurationFileName + " could not be written. The settings are kept in memory for this session. Reason: " + ex.Message);
        }
    }

    private static bool HasEverySetting(string path, ModConfiguration configuration)
    {
        XDocument complete = new();
        using (XmlWriter writer = complete.CreateWriter())
        {
            Serializer.Serialize(writer, configuration, NoNamespaces());
        }

        XElement? fileRoot = XDocument.Load(path).Root;
        return fileRoot != null && complete.Root != null && ContainsEveryElement(fileRoot, complete.Root);
    }

    private static bool ContainsEveryElement(XElement actual, XElement expected)
    {
        foreach (XElement expectedChild in expected.Elements())
        {
            XElement? actualChild = actual.Element(expectedChild.Name);
            if (actualChild == null || !ContainsEveryElement(actualChild, expectedChild)) return false;
        }

        return true;
    }

    private static XmlSerializerNamespaces NoNamespaces()
    {
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add(string.Empty, string.Empty);
        return namespaces;
    }
}
