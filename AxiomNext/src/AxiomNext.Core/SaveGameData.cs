using System.Text.Json;

namespace AxiomNext.Core;

/// <summary>
/// Versioned, engine-independent save format suitable for local Android storage.
/// </summary>
public sealed record SaveGameData
{
    public int SaveVersion { get; init; } = 1;
    public string PlayerName { get; init; } = "Player";
    public int Health { get; init; } = 100;
    public int Coins { get; init; }
    public int HighestUnlockedLevel { get; init; } = 1;
    public List<int> CompletedLevels { get; init; } = [];
    public List<string> CollectedCrystals { get; init; } = [];
    public List<string> CollectedArtifacts { get; init; } = [];
    public bool SecretWorldUnlocked { get; init; }
    public CheckpointData? Checkpoint { get; init; }

    public string ToJson() => JsonSerializer.Serialize(this, JsonOptions);

    public static SaveGameData FromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("Save data is empty.", nameof(json));

        var data = JsonSerializer.Deserialize<SaveGameData>(json, JsonOptions)
            ?? throw new InvalidOperationException("Save data could not be read.");

        if (data.SaveVersion != 1)
            throw new InvalidOperationException($"Unsupported save version: {data.SaveVersion}.");
        if (string.IsNullOrWhiteSpace(data.PlayerName))
            throw new InvalidOperationException("Save data has no player name.");
        if (data.Coins < 0 || data.Health is < 0 or > 100)
            throw new InvalidOperationException("Save data contains invalid player statistics.");

        return data;
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
}
