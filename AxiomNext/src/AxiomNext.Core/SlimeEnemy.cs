namespace AxiomNext.Core;

public enum SlimeArchetype
{
    Scout,
    Heavy,
    Hunter
}

/// <summary>
/// Enemy combat profile. Stride is responsible for navigation and animation.
/// </summary>
public sealed class SlimeEnemy
{
    public SlimeEnemy(
        string id,
        int health = 30,
        int contactDamage = 10,
        int coinReward = 5,
        float moveSpeed = 3f,
        float attackRange = 1.8f,
        bool retreatsWhenWeak = false,
        SlimeArchetype archetype = SlimeArchetype.Scout)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Enemy id cannot be empty.", nameof(id));
        if (health <= 0) throw new ArgumentOutOfRangeException(nameof(health));
        if (contactDamage < 0) throw new ArgumentOutOfRangeException(nameof(contactDamage));
        if (coinReward < 0) throw new ArgumentOutOfRangeException(nameof(coinReward));
        if (moveSpeed < 0) throw new ArgumentOutOfRangeException(nameof(moveSpeed));
        if (attackRange < 0) throw new ArgumentOutOfRangeException(nameof(attackRange));

        Id = id;
        MaxHealth = health;
        Health = health;
        ContactDamage = contactDamage;
        CoinReward = coinReward;
        MoveSpeed = moveSpeed;
        AttackRange = attackRange;
        RetreatsWhenWeak = retreatsWhenWeak;
        Archetype = archetype;
    }

    public string Id { get; }
    public SlimeArchetype Archetype { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public int ContactDamage { get; }
    public int CoinReward { get; }
    public float MoveSpeed { get; }
    public float AttackRange { get; }
    public bool RetreatsWhenWeak { get; }
    public bool IsDefeated => Health == 0;
    public int RetreatHealthThreshold => Math.Max(1, MaxHealth / 4);

    public int TakeDamage(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (IsDefeated || amount == 0) return 0;
        var applied = Math.Min(Health, amount);
        Health -= applied;
        return applied;
    }

    public static SlimeEnemy CreateScout(string id) =>
        new(id, 60, 10, 4, 5f, 1.8f, true, SlimeArchetype.Scout);

    public static SlimeEnemy CreateHeavy(string id) =>
        new(id, 200, 30, 12, 1.8f, 2.2f, false, SlimeArchetype.Heavy);

    public static SlimeEnemy CreateHunter(string id) =>
        new(id, 90, 20, 8, 2.5f, 7f, true, SlimeArchetype.Hunter);
}
