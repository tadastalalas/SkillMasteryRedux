using System.Collections.Generic;
using SkillMasteryRedux.Core;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;

namespace SkillMasteryRedux.UI;

// Character screen hero dropdown row; HeroSelectorMarkerPrefab binds the Smr* properties to the unspent-points circle and the perk shield.
public sealed class HeroSelectorItemVM : SelectorItemVM
{
    private const string AttributePointsProperty = "UnspentAttributePoints";
    private const string FocusPointsProperty = "UnspentCharacterPoints";
    private const string UnopenedPerksProperty = "NumOfUnopenedPerks";
    private const string SkillsProperty = "Skills";
    private const string PerksProperty = "Perks";

    private readonly CharacterDeveloperHeroItemVM _hero;
    private readonly List<SkillVM> _skills = [];
    private MBBindingList<SkillVM>? _skillList;
    private bool _isTracking;
    private bool _smrHasUnspentPoints;
    private bool _smrHasUnopenedPerks;
    private int _smrUnopenedPerks;

    // isTracking false = markers stay hidden and no vanilla events are hooked.
    public HeroSelectorItemVM(CharacterDeveloperHeroItemVM hero, bool isTracking) : base(hero.HeroNameText)
    {
        _hero = hero;
        if (!isTracking) return;
        _isTracking = true;
        hero.PropertyChangedWithIntValue += OnHeroIntValueChanged;
        hero.PropertyChangedWithValue += OnHeroValueChanged;
        HookSkills();
        RefreshMarkers();
    }

    [DataSourceProperty]
    public bool SmrHasUnspentPoints
    {
        get => _smrHasUnspentPoints;
        set
        {
            if (value == _smrHasUnspentPoints) return;
            _smrHasUnspentPoints = value;
            OnPropertyChangedWithValue(value, nameof(SmrHasUnspentPoints));
        }
    }

    [DataSourceProperty]
    public bool SmrHasUnopenedPerks
    {
        get => _smrHasUnopenedPerks;
        set
        {
            if (value == _smrHasUnopenedPerks) return;
            _smrHasUnopenedPerks = value;
            OnPropertyChangedWithValue(value, nameof(SmrHasUnopenedPerks));
        }
    }

    [DataSourceProperty]
    public int SmrUnopenedPerks
    {
        get => _smrUnopenedPerks;
        set
        {
            if (value == _smrUnopenedPerks) return;
            _smrUnopenedPerks = value;
            OnPropertyChangedWithValue(value, nameof(SmrUnopenedPerks));
        }
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        Unhook();
    }

    internal void Unhook()
    {
        if (!_isTracking) return;
        _isTracking = false;
        _hero.PropertyChangedWithIntValue -= OnHeroIntValueChanged;
        _hero.PropertyChangedWithValue -= OnHeroValueChanged;
        UnhookSkills();
    }

    private void HookSkills()
    {
        _skillList = _hero.Skills;
        if (_skillList == null) return;
        _skillList.ListChanged += OnSkillListChanged;
        HookSkillItems();
    }

    private void HookSkillItems()
    {
        if (_skillList == null) return;
        for (int i = 0; i < _skillList.Count; i++)
        {
            SkillVM skill = _skillList[i];
            if (skill == null) continue;
            skill.PropertyChangedWithIntValue += OnSkillIntValueChanged;
            skill.PropertyChangedWithValue += OnSkillValueChanged;
            _skills.Add(skill);
        }
    }

    private void UnhookSkills()
    {
        if (_skillList != null) _skillList.ListChanged -= OnSkillListChanged;
        _skillList = null;
        UnhookSkillItems();
    }

    private void UnhookSkillItems()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].PropertyChangedWithIntValue -= OnSkillIntValueChanged;
            _skills[i].PropertyChangedWithValue -= OnSkillValueChanged;
        }
        _skills.Clear();
    }

    private void RehookSkills()
    {
        UnhookSkills();
        HookSkills();
        RefreshMarkers();
    }

    private void RefreshMarkers()
    {
        SmrHasUnspentPoints = _hero.UnspentAttributePoints > 0 || _hero.UnspentCharacterPoints > 0;
        int perks = HasBuiltPerks() ? _hero.GetNumberOfUnselectedPerks() : CountFromGameData();
        SmrUnopenedPerks = perks;
        SmrHasUnopenedPerks = perks > 0;
    }

    private bool HasBuiltPerks()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].Perks?.Count > 0) return true;
        }
        return false;
    }

    private int CountFromGameData() => _hero.Hero == null ? 0 : UnopenedPerkCounter.Count(_hero.Hero, _hero.PerkSelection);

    private void OnHeroIntValueChanged(object sender, PropertyChangedWithIntValueEventArgs e)
    {
        if (e.PropertyName == AttributePointsProperty || e.PropertyName == FocusPointsProperty) RefreshMarkers();
    }

    private void OnHeroValueChanged(object sender, PropertyChangedWithValueEventArgs e)
    {
        if (e.PropertyName == SkillsProperty) RehookSkills();
    }

    // MBBindingList walks its ListChanged listeners with foreach: never add or remove a ListChanged listener in here, only re-hook the items.
    private void OnSkillListChanged(object sender, ListChangedEventArgs e)
    {
        if (e.ListChangedType == ListChangedType.ItemBeforeDeleted) return;
        UnhookSkillItems();
        HookSkillItems();
        RefreshMarkers();
    }

    private void OnSkillIntValueChanged(object sender, PropertyChangedWithIntValueEventArgs e)
    {
        if (e.PropertyName == UnopenedPerksProperty) RefreshMarkers();
    }

    private void OnSkillValueChanged(object sender, PropertyChangedWithValueEventArgs e)
    {
        if (e.PropertyName == PerksProperty) RefreshMarkers();
    }
}
