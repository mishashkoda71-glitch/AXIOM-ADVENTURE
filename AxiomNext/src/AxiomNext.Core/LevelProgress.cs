namespace AxiomNext.Core;

/// <summary>
/// Tracks unlocks and completion for a fixed-size campaign.
/// Level numbers are one-based for player-facing UI.
/// </summary>
public sealed class LevelProgress
{
    private readonly bool[] _completed;

    public LevelProgress(int levelCount)
    {
        if (levelCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(levelCount));

        _completed = new bool[levelCount];
    }

    public int LevelCount => _completed.Length;
    public int HighestUnlockedLevel { get; private set; } = 1;
    public int CompletedCount => _completed.Count(done => done);

    public bool IsUnlocked(int level) =>
        level >= 1 && level <= HighestUnlockedLevel;

    public bool IsCompleted(int level) =>
        level >= 1 && level <= LevelCount && _completed[level - 1];

    public bool Complete(int level)
    {
        if (!IsUnlocked(level))
            return false;

        _completed[level - 1] = true;

        if (level < LevelCount)
            HighestUnlockedLevel = Math.Max(HighestUnlockedLevel, level + 1);

        return true;
    }
}
