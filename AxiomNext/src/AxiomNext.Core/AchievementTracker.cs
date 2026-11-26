namespace AxiomNext.Core;

public sealed record Achievement(string Id, string Title, string Description);

/// <summary>
/// Evaluates the original Axiom achievement milestones from current campaign state.
/// </summary>
public static class AchievementTracker
{
    private static readonly Achievement[] All =
    [
        new("first-crystal", "First Spark", "Collect your first crystal."),
        new("ten-crystals", "Crystal Collector", "Collect 10 different crystals."),
        new("level-10", "Pathfinder", "Unlock level 10."),
        new("level-20", "Trailblazer", "Unlock level 20."),
        new("level-40", "Axiom Champion", "Unlock the final level."),
        new("secret-world", "Hidden Horizon", "Discover the secret world."),
        new("first-artifact", "Ancient Mystery", "Find your first secret artifact.")
    ];

    public static IReadOnlyList<Achievement> Evaluate(GameSession game)
    {
        ArgumentNullException.ThrowIfNull(game);
        var unlockedLevel = game.Progress.HighestUnlockedLevel;
        var crystals = game.Player.CollectedCrystals.Count;

        var flags = new[]
        {
            crystals >= 1,
            crystals >= 10,
            unlockedLevel >= 10,
            unlockedLevel >= 20,
            unlockedLevel >= GameSession.CampaignLevelCount,
            game.SecretWorldUnlocked,
            game.CollectedArtifacts.Count >= 1
        };

        return All.Where((_, index) => flags[index]).ToArray();
    }
}
