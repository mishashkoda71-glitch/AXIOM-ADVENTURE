namespace AxiomNext.Core;

/// <summary>
/// Engine-independent game session state.
/// Rendering, input and networking will be supplied by platform adapters.
/// </summary>
public sealed class GameSession
{
    public const int CampaignLevelCount = 40;

    public GameSession(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            throw new ArgumentException("Player name cannot be empty.", nameof(playerName));

        Player = new PlayerState(playerName.Trim());
        Progress = new LevelProgress(CampaignLevelCount);
    }

    public PlayerState Player { get; }
    public LevelProgress Progress { get; }
    public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    public void SetHealth(int value)
    {
        Player.Health = Math.Clamp(value, 0, Player.MaxHealth);
    }

    public bool CollectCrystal(string crystalId)
    {
        if (string.IsNullOrWhiteSpace(crystalId))
            return false;

        return Player.CollectedCrystals.Add(crystalId);
    }
}

public sealed class PlayerState
{
    public PlayerState(string name) => Name = name;

    public string Name { get; }
    public int MaxHealth { get; } = 100;
    public int Health { get; internal set; } = 100;
    public int Coins { get; private set; }
    public HashSet<string> CollectedCrystals { get; } = new(StringComparer.Ordinal);

    public void AddCoins(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Coins = checked(Coins + amount);
    }

    public bool SpendCoins(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        if (Coins < amount)
            return false;

        Coins -= amount;
        return true;
    }
}
