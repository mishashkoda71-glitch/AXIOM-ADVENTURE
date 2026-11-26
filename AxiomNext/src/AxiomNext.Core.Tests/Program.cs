using AxiomNext.Core;

static void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException($"TEST FAILED: {message}");
}

var game = new GameSession("  Explorer  ");
Check(game.Player.Name == "Explorer", "player name is trimmed");
Check(game.Player.Health == 100, "player starts with full health");
Check(game.DamagePlayer(35) == 35 && game.Player.Health == 65, "damage reduces health");
Check(game.HealPlayer(20) == 20 && game.Player.Health == 85, "healing restores health");
Check(game.HealPlayer(50) == 15 && game.Player.Health == 100, "health is capped");
Check(game.CollectCrystal("blue-01"), "first crystal pickup succeeds");
Check(!game.CollectCrystal("blue-01"), "duplicate crystal pickup is ignored");
for (var i = 2; i <= 10; i++) game.CollectCrystal($"crystal-{i:00}");

var slime = new SlimeEnemy("slime-01", health: 25, contactDamage: 8, coinReward: 7);
Check(!game.AttackEnemy(slime, 10), "slime survives first hit");
Check(game.AttackEnemy(slime, 20), "second hit defeats slime");
Check(game.Player.Coins == 7, "enemy reward is granted");
Check(!game.AttackEnemy(slime, 20), "defeated slime cannot be rewarded twice");

Check(game.CompleteLevel(1), "level one completes");
Check(game.Progress.IsUnlocked(2), "level two unlocks");
Check(!game.CompleteLevel(3), "locked levels cannot be completed");
Check(game.ActivateCheckpoint("Level_01", 1.5f, 2f, -3f), "checkpoint activates");
Check(game.Checkpoint?.LevelId == "Level_01", "checkpoint level is stored");
Check(game.CollectArtifact("artifact-ancient"), "artifact is collected");
Check(!game.CollectArtifact("artifact-ancient"), "duplicate artifact is ignored");
Check(game.UnlockSecretWorld(), "secret world unlocks");
Check(!game.UnlockSecretWorld(), "secret world cannot be unlocked twice");
Check(AchievementTracker.Evaluate(game).Any(a => a.Id == "first-crystal"), "first crystal achievement unlocks");
Check(AchievementTracker.Evaluate(game).Any(a => a.Id == "ten-crystals"), "ten crystal achievement unlocks");
Check(AchievementTracker.Evaluate(game).Any(a => a.Id == "first-artifact"), "artifact achievement unlocks");
Check(AchievementTracker.Evaluate(game).Any(a => a.Id == "secret-world"), "secret world achievement unlocks");

var restored = GameSession.Restore(SaveGameData.FromJson(game.CreateSaveData().ToJson()));
Check(restored.Player.Name == "Explorer", "save restores player name");
Check(restored.Player.Health == 100, "save restores health");
Check(restored.Player.Coins == 7, "save restores coins");
Check(restored.Player.CollectedCrystals.Count == 10, "save restores crystals");
Check(restored.Progress.IsCompleted(1) && restored.Progress.IsUnlocked(2), "save restores level progress");
Check(restored.Checkpoint?.LevelId == "Level_01" && restored.Checkpoint.X == 1.5f, "save restores checkpoint");
Check(restored.CollectedArtifacts.Contains("artifact-ancient"), "save restores artifacts");
Check(restored.SecretWorldUnlocked, "save restores secret world state");

game.Pause();
Check(game.DamagePlayer(10) == 0, "paused game ignores damage");
game.Resume();
Check(game.DamagePlayer(200) == 100 && game.Player.Health == 0, "health cannot drop below zero");
Check(game.HealPlayer(10) == 0, "defeated player cannot heal");

Console.WriteLine("All AxiomNext.Core checks passed.");
