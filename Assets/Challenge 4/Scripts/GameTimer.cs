using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public UIManager uiManager;
    public TMP_Text timerText;

    private float timeRemaining;
    private bool timerRunning = false;

    private void Start()
    {
        // Do not start automatically
        timerRunning = false;
        UpdateTimerUI(0);
    }

    private void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            EndMatch();
        }

        UpdateTimerUI(timeRemaining);
    }

    public void StartTimer()
    {
        timeRemaining = GameSettings.MatchDuration;
        timerRunning = true;
    }

    private void UpdateTimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void EndMatch()
    {
        Time.timeScale = 0f;

        // Decide winner based on score
        int playerScore = uiManager.GetPlayerScore();
        int enemyScore = uiManager.GetEnemyScore();

        bool playerWon = playerScore >= enemyScore;

        uiManager.ShowGameOverPanel(playerWon);
    }
}