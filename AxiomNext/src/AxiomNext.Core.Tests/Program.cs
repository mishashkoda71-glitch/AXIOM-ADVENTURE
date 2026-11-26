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

var slime = new SlimeEnemy("slime-01", health: 25, contactDamage: 8, coinReward: 7);
Check(!game.AttackEnemy(slime, 10), "slime survives first hit");
Check(game.AttackEnemy(slime, 20), "second hit defeats slime");
Check(game.Player.Coins == 7, "enemy reward is granted");
Check(!game.AttackEnemy(slime, 20), "defeated slime cannot be rewarded twice");

Check(game.CompleteLevel(1), "level one completes");
Check(game.Progress.IsUnlocked(2), "level two unlocks");
Check(!game.CompleteLevel(3), "locked levels cannot be completed");

var restored = GameSession.Restore(SaveGameData.FromJson(game.CreateSaveData().ToJson()));
Check(restored.Player.Name == "Explorer", "save restores player name");
Check(restored.Player.Health == 100, "save restores health");
Check(restored.Player.Coins == 7, "save restores coins");
Check(restored.Player.CollectedCrystals.Contains("blue-01"), "save restores crystals");
Check(restored.Progress.IsCompleted(1) && restored.Progress.IsUnlocked(2), "save restores level progress");

game.Pause();
Check(game.DamagePlayer(10) == 0, "paused game ignores damage");
game.Resume();
Check(game.DamagePlayer(200) == 100 && game.Player.Health == 0, "health cannot drop below zero");
Check(game.HealPlayer(10) == 0, "defeated player cannot heal");

Console.WriteLine("All AxiomNext.Core checks passed.");
