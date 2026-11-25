namespace SkillMasteryRedux.Core;

// Every attribute bonus, in character screen order (Vigor, Control, Endurance, Cunning, Social, Intelligence).
// The order is the MCM order and the attribute tooltip order.
internal enum AttributeBonus
{
    // Vigor
    MeleeDamage,
    DrawSpeed,
    CrushThrough,
    SliceThrough,

    // Control
    RangedDamage,
    Accuracy,
    ReloadSpeed,
    WeaponHandling,

    // Endurance
    MaxHitPoints,
    StaggerResistance,
    AimStability,
    MovementSpeed,
    SmithingStamina,

    // Cunning
    SimulationAdvantage,
    VictoryRenown,
    VictoryMorale,

    // Social
    PersuasionChance,
    PartyMorale,
    PartySize,
    PartyWages,
    CompanionLimit,

    // Intelligence
    ClanIncome,
    VictoryInfluence,
    ExperienceGain,
    RoleExperience,
    HeroHealing,
}
