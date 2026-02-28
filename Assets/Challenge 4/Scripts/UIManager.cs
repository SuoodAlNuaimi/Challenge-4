using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public GameObject helpPanel;

    [Header("In Game UIs")]
    public TMP_Text timerText;
    public TMP_Text player_Score;
    public TMP_Text enemy_Score;
    public GameObject smashPowerHint;
    public Slider normalPowerSlider;
    public Slider smashPowerSlider;
    public TMP_Text normalPowerUpStatus; // active or cooldown
    public TMP_Text smashPowerUpStatus; // active or cooldown

    [Header("Game Over UIs")]
    public TMP_Text gameStatusText;

    public bool isGameStarted = false;

    public GameTimer gameTimer;
    public PlayerControllerX playerController;

    private int playerScoreValue = 0;
    private int enemyScoreValue = 0;

    public void Awake()
    {
        Instance= this;
        Time.timeScale = 0f;
        isGameStarted = false;
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }
    public void StartGameWithDifficulty()
    {
        ShowInGamePanel();
    }

    public void ShowHelpPanel()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void ShowDifficultyPanel()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        inGamePanel.SetActive(false);
        helpPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowInGamePanel()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        inGamePanel.SetActive(true);
        helpPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameStarted = true;
        gameTimer.StartTimer();
        playerController.OnGameStart();
    }

    public void ShowGameOverPanel(bool playerWon)
    {
        // false all and show only game over panel
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        inGamePanel.SetActive(false);
        helpPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameStatusText.text = playerWon ? "You Win!" : "You Lose!";
    }

    public void ShowMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        inGamePanel.SetActive(false);
        helpPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false; // For editor testing
    }

    public void AddPlayerScore()
    {
        playerScoreValue++;
        player_Score.text = "Player: "+playerScoreValue.ToString();
    }

    public void AddEnemyScore()
    {
        enemyScoreValue++;
        enemy_Score.text = "Enemy: " + enemyScoreValue.ToString();
    }

    public void UpdateNormalPowerSlider(float value)
    {
        normalPowerSlider.value = value;
    }

    public void UpdateSmashPowerSlider(float value)
    {
        smashPowerSlider.value = value;
    }
    public void SetNormalPowerStatus(bool active)
    {
        normalPowerUpStatus.text = active ? "ACTIVE" : "READY";
    }

    public void SetSmashPowerStatus(bool active)
    {
        smashPowerUpStatus.text = active ? "ACTIVE" : "READY";
        smashPowerHint.SetActive(active);
    }
}
