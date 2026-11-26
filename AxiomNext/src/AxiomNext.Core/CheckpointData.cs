namespace AxiomNext.Core;

/// <summary>
/// A stable world position that can be restored after the player is defeated.
/// Coordinates are stored as floats to keep saves engine-independent.
/// </summary>
public sealed record CheckpointData(string LevelId, float X, float Y, float Z);
