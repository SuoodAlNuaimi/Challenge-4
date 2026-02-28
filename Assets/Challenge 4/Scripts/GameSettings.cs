public static class GameSettings
{
    public static Difficulty CurrentDifficulty = Difficulty.Medium;

    public static float MatchDuration;

    // Player
    public static float PlayerMoveForce;
    public static float TurboForce;
    public static float PowerupDuration;
    public static float SmashRadiusMultiplier;

    // Enemy
    public static float EnemySpeedMultiplier;
    public static float EnemyMaxSpeed;
    public static float EnemyDecisionRate;
    public static float InterceptPredictionMultiplier;

    // Spawn
    public static bool ExtraEnemyPerWave;
}