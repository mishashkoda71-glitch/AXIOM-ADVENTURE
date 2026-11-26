namespace AxiomNext.Core;

/// <summary>
/// Engine-independent rules and state for one adventure session.
/// Stride supplies rendering, physics, input and audio.
/// </summary>
public sealed class GameSession
{
    public const int CampaignLevelCount = 40;
    public const int StartingHealth = 100;

    public GameSession(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            throw new ArgumentException("Player name cannot be empty.", nameof(playerName));

        Player = new PlayerState(playerName.Trim());
        Progress = new LevelProgress(CampaignLevelCount);
    }

    private GameSession(SaveGameData save) : this(save.PlayerName)
    {
        SetHealth(save.Health);
        Player.AddCoins(save.Coins);
        foreach (var crystalId in save.CollectedCrystals.Where(x => !string.IsNullOrWhiteSpace(x)))
            Player.CollectedCrystals.Add(crystalId);
        foreach (var level in save.CompletedLevels.Distinct().Order())
            if (Progress.IsUnlocked(level)) Progress.Complete(level);
        while (Progress.HighestUnlockedLevel < Math.Clamp(save.HighestUnlockedLevel, 1, CampaignLevelCount))
            Progress.Complete(Progress.HighestUnlockedLevel);
    }

    public PlayerState Player { get; }
    public LevelProgress Progress { get; }
    public bool IsPaused { get; private set; }
    public int DefeatedEnemyCount { get; private set; }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;
    public void SetHealth(int value) => Player.Health = Math.Clamp(value, 0, Player.MaxHealth);

    public int DamagePlayer(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (IsPaused || Player.Health == 0) return 0;
        var applied = Math.Min(Player.Health, amount);
        Player.Health -= applied;
        return applied;
    }

    public int HealPlayer(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (IsPaused || Player.Health == 0) return 0;
        var restored = Math.Min(Player.MaxHealth - Player.Health, amount);
        Player.Health += restored;
        return restored;
    }

    public void CollectCoins(int amount = 1) => Player.AddCoins(amount);

    public bool CollectCrystal(string crystalId)
    {
        if (string.IsNullOrWhiteSpace(crystalId)) return false;
        return Player.CollectedCrystals.Add(crystalId);
    }

    public bool AttackEnemy(SlimeEnemy enemy, int damage)
    {
        ArgumentNullException.ThrowIfNull(enemy);
        if (IsPaused || Player.Health == 0 || enemy.IsDefeated) return false;
        enemy.TakeDamage(damage);
        if (!enemy.IsDefeated) return false;
        DefeatedEnemyCount++;
        Player.AddCoins(enemy.CoinReward);
        return true;
    }

    public bool CompleteLevel(int level) => Progress.Complete(level);

    public SaveGameData CreateSaveData() => new()
    {
        PlayerName = Player.Name,
        Health = Player.Health,
        Coins = Player.Coins,
        HighestUnlockedLevel = Progress.HighestUnlockedLevel,
        CompletedLevels = Enumerable.Range(1, Progress.LevelCount).Where(Progress.IsCompleted).ToList(),
        CollectedCrystals = Player.CollectedCrystals.Order(StringComparer.Ordinal).ToList()
    };

    public static GameSession Restore(SaveGameData save)
    {
        ArgumentNullException.ThrowIfNull(save);
        return new GameSession(save);
    }
}

public sealed class PlayerState
{
    public PlayerState(string name) => Name = name;
    public string Name { get; }
    public int MaxHealth { get; } = GameSession.StartingHealth;
    public int Health { get; internal set; } = GameSession.StartingHealth;
    public int Coins { get; private set; }
    public HashSet<string> CollectedCrystals { get; } = new(StringComparer.Ordinal);

    public void AddCoins(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Coins = checked(Coins + amount);
    }

    public bool SpendCoins(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (Coins < amount) return false;
        Coins -= amount;
        return true;
    }
}
