using UnityEngine;
using UnityEngine.UI;

public class DifficultyUIScreen : MonoBehaviour
{
    public Button[] modeButtons;      // 0 Easy, 1 Medium, 2 Hard
    public Button[] timerButtons;     // 0-2min, 1-3min, 2-5min, 3-7min

    public Button startButton;
    public Color selectedColor;

    public DifficultyConfigurator difficultyConfigurator;
    public UIManager uiManager;

    private int selectedModeIndex = -1;
    private int selectedTimerIndex = -1;

    private Color[] normalModeButtonColors;
    private Color[] normalTimerButtonColors;

    private void Start()
    {
        normalModeButtonColors = new Color[modeButtons.Length];
        for (int i = 0; i < modeButtons.Length; i++)
            normalModeButtonColors[i] = modeButtons[i].image.color;

        normalTimerButtonColors = new Color[timerButtons.Length];
        for (int i = 0; i < timerButtons.Length; i++)
            normalTimerButtonColors[i] = timerButtons[i].image.color;

        startButton.interactable = false;

        // buttons listen to clicks
        for (int i = 0; i < modeButtons.Length; i++)
        {
            int index = i; // Capture index for lambda
            modeButtons[i].onClick.AddListener(() => OnModeButtonClicked(index));
        }

        for (int i = 0; i < timerButtons.Length; i++)
        {
            int index = i; // Capture index for lambda
            timerButtons[i].onClick.AddListener(() => OnTimerButtonClicked(index));
        }
    }

    public void OnModeButtonClicked(int index)
    {
        selectedModeIndex = index;
        UpdateButtonColors(modeButtons, normalModeButtonColors, selectedModeIndex);

        switch (index)
        {
            case 0: difficultyConfigurator.SetEasy(); break;
            case 1: difficultyConfigurator.SetMedium(); break;
            case 2: difficultyConfigurator.SetHard(); break;
        }

        CheckStartButtonInteractable();
    }

    public void OnTimerButtonClicked(int index)
    {
        selectedTimerIndex = index;
        UpdateButtonColors(timerButtons, normalTimerButtonColors, selectedTimerIndex);

        switch (index)
        {
            case 0: GameSettings.MatchDuration = 120f; break;
            case 1: GameSettings.MatchDuration = 180f; break;
            case 2: GameSettings.MatchDuration = 300f; break;
            case 3: GameSettings.MatchDuration = 420f; break;
        }

        CheckStartButtonInteractable();
    }

    private void UpdateButtonColors(Button[] buttons, Color[] normalColors, int selectedIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.color = (i == selectedIndex) ? selectedColor : normalColors[i];
        }
    }

    private void CheckStartButtonInteractable()
    {
        startButton.interactable = (selectedModeIndex != -1 && selectedTimerIndex != -1);
    }

    public void OnStartButtonClicked()
    {
        uiManager.StartGameWithDifficulty();
    }

    public void ResetSelections()
    {
        selectedModeIndex = -1;
        selectedTimerIndex = -1;
        UpdateButtonColors(modeButtons, normalModeButtonColors, selectedModeIndex);
        UpdateButtonColors(timerButtons, normalTimerButtonColors, selectedTimerIndex);
        startButton.interactable = false;
    }
}