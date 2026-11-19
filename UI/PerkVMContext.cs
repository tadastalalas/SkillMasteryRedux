using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using SkillMasteryRedux.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;

namespace SkillMasteryRedux.UI;

// Resolves the hero shown in the character screen (and the owning SkillVM) from a PerkVM:
// vanilla hands PerkVM a _getIsPerkSelected delegate whose target (SkillVM / closure) reaches CharacterVM.Hero.
internal static class PerkVMContext
{
    private const BindingFlags InstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private const string HeroPropertyName = "Hero";

    // Fragile: private vanilla names, re-verify in dnSpy on game updates. Null = feature inert, logged once.
    private static readonly FieldInfo? GetIsPerkSelectedField = AccessTools.Field(typeof(PerkVM), "_getIsPerkSelected");
    private static readonly MethodInfo? RefreshNumOfUnopenedPerksMethod = AccessTools.Method(typeof(SkillVM), "RefreshNumOfUnopenedPerks");

    private static readonly Dictionary<Type, Func<object, Hero?>?> HeroGetters = new();
    private static readonly Dictionary<Type, Func<object, SkillVM?>?> SkillGetters = new();

    internal static Hero? GetHero(PerkVM vm)
    {
        object? target = GetDelegateTarget(vm);
        if (target == null) return null;
        Type type = target.GetType();
        if (!HeroGetters.TryGetValue(type, out Func<object, Hero?>? getter))
        {
            getter = BuildHeroGetter(type);
            HeroGetters[type] = getter;
            if (getter == null) SmrLog.Warn($"No Hero accessor on {type.FullName}; mastery UI inert for perks created by it.");
        }
        return getter == null ? null : Invoke(getter, target, type);
    }

    // Best effort: re-runs the skill's unopened-perk count after a manual mastery unlock (goes through SkillVMPatch).
    internal static void RefreshUnopenedPerks(PerkVM vm)
    {
        if (RefreshNumOfUnopenedPerksMethod == null) return;
        SkillVM? skillVm = GetSkillVM(vm);
        if (skillVm != null) RefreshNumOfUnopenedPerksMethod.Invoke(skillVm, null);
    }

    private static SkillVM? GetSkillVM(PerkVM vm)
    {
        object? target = GetDelegateTarget(vm);
        if (target == null) return null;
        if (target is SkillVM direct) return direct;
        Type type = target.GetType();
        if (!SkillGetters.TryGetValue(type, out Func<object, SkillVM?>? getter))
        {
            getter = BuildSkillGetter(type);
            SkillGetters[type] = getter;
        }
        return getter == null ? null : Invoke(getter, target, type);
    }

    private static object? GetDelegateTarget(PerkVM vm)
    {
        if (GetIsPerkSelectedField == null)
        {
            SmrLog.WarnOnce("PerkVM._getIsPerkSelected", "PerkVM._getIsPerkSelected not found; mastery UI inert.");
            return null;
        }
        return (GetIsPerkSelectedField.GetValue(vm) as Delegate)?.Target;
    }

    // Reflection into vanilla/closure types: an exception here must not escape into the patched UI method.
    private static T? Invoke<T>(Func<object, T?> getter, object target, Type type) where T : class
    {
        try
        {
            return getter(target);
        }
        catch (Exception ex)
        {
            SmrLog.WarnOnce("ctx:" + type.FullName, $"Context lookup failed on {type.FullName}: {ex.GetType().Name}: {ex.Message}");
            return null;
        }
    }

    private static Func<object, Hero?>? BuildHeroGetter(Type type)
    {
        PropertyInfo? direct = FindHeroProperty(type);
        if (direct != null) return obj => direct.GetValue(obj, null) as Hero;
        foreach (FieldInfo field in type.GetFields(InstanceAll))
        {
            PropertyInfo? heroProperty = FindHeroProperty(field.FieldType);
            if (heroProperty != null) return obj => Read(heroProperty, field.GetValue(obj));
        }
        foreach (PropertyInfo property in type.GetProperties(InstanceAll))
        {
            if (property.GetIndexParameters().Length != 0) continue;
            PropertyInfo? heroProperty = FindHeroProperty(property.PropertyType);
            if (heroProperty != null) return obj => Read(heroProperty, property.GetValue(obj, null));
        }
        return null;
    }

    private static Func<object, SkillVM?>? BuildSkillGetter(Type type)
    {
        foreach (FieldInfo field in type.GetFields(InstanceAll))
        {
            if (typeof(SkillVM).IsAssignableFrom(field.FieldType)) return obj => field.GetValue(obj) as SkillVM;
        }
        return null;
    }

    private static PropertyInfo? FindHeroProperty(Type type)
    {
        try
        {
            PropertyInfo? property = type.GetProperty(HeroPropertyName, InstanceAll);
            return property != null && property.PropertyType == typeof(Hero) ? property : null;
        }
        catch (AmbiguousMatchException)
        {
            return null;
        }
    }

    private static Hero? Read(PropertyInfo heroProperty, object? owner) => owner == null ? null : heroProperty.GetValue(owner, null) as Hero;
}