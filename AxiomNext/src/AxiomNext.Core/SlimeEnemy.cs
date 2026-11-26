namespace AxiomNext.Core;

/// <summary>
/// A lightweight enemy model. Animation and movement are handled by Stride.
/// </summary>
public sealed class SlimeEnemy
{
    public SlimeEnemy(string id, int health = 30, int contactDamage = 10, int coinReward = 5)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Enemy id cannot be empty.", nameof(id));
        if (health <= 0)
            throw new ArgumentOutOfRangeException(nameof(health));
        if (contactDamage < 0)
            throw new ArgumentOutOfRangeException(nameof(contactDamage));
        if (coinReward < 0)
            throw new ArgumentOutOfRangeException(nameof(coinReward));

        Id = id;
        MaxHealth = health;
        Health = health;
        ContactDamage = contactDamage;
        CoinReward = coinReward;
    }

    public string Id { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public int ContactDamage { get; }
    public int CoinReward { get; }
    public bool IsDefeated => Health == 0;

    public int TakeDamage(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        if (IsDefeated || amount == 0)
            return 0;

        var applied = Math.Min(Health, amount);
        Health -= applied;
        return applied;
    }
}
