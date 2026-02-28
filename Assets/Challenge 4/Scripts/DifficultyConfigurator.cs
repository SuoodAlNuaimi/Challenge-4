using UnityEngine;

public class DifficultyConfigurator : MonoBehaviour
{
    public void SetEasy()
    {
        GameSettings.CurrentDifficulty = Difficulty.Easy;

        // Player
        GameSettings.PlayerMoveForce = 550f;
        GameSettings.TurboForce = 13f;
        GameSettings.PowerupDuration = 7f;
        GameSettings.SmashRadiusMultiplier = 1.2f;

        // Enemy
        GameSettings.EnemySpeedMultiplier = 0.15f;
        GameSettings.EnemyMaxSpeed = 5.5f;
        GameSettings.EnemyDecisionRate = 0.4f;
        GameSettings.InterceptPredictionMultiplier = 0.4f;

        // Spawn
        GameSettings.ExtraEnemyPerWave = false;
    }

    public void SetMedium()
    {
        GameSettings.CurrentDifficulty = Difficulty.Medium;

        GameSettings.PlayerMoveForce = 500f;
        GameSettings.TurboForce = 10f;
        GameSettings.PowerupDuration = 5f;
        GameSettings.SmashRadiusMultiplier = 1f;

        GameSettings.EnemySpeedMultiplier = 0.25f;
        GameSettings.EnemyMaxSpeed = 7f;
        GameSettings.EnemyDecisionRate = 0.25f;
        GameSettings.InterceptPredictionMultiplier = 0.5f;

        GameSettings.ExtraEnemyPerWave = false;
    }

    public void SetHard()
    {
        GameSettings.CurrentDifficulty = Difficulty.Hard;

        GameSettings.PlayerMoveForce = 480f;
        GameSettings.TurboForce = 9f;
        GameSettings.PowerupDuration = 4f;
        GameSettings.SmashRadiusMultiplier = 0.9f;

        GameSettings.EnemySpeedMultiplier = 0.4f;
        GameSettings.EnemyMaxSpeed = 9f;
        GameSettings.EnemyDecisionRate = 0.15f;
        GameSettings.InterceptPredictionMultiplier = 1f;

        GameSettings.ExtraEnemyPerWave = true;
    }
}