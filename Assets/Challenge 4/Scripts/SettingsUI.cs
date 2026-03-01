using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Button musicButton;
    public Button soundsButton;

    [Header("Music Sprites")]
    public Image musicOnSprite;
    public Image musicOffSprite;

    [Header("Sounds Sprites")]
    public Image soundsOnSprite;
    public Image soundsOffSprite;

    public void Awake()
    {
        musicButton.onClick.AddListener(ToggleMusic);
        soundsButton.onClick.AddListener(ToggleSounds);
    }
    private void OnEnable()
    {
        UpdateUI();
    }

    public void ToggleMusic()
    {
        SoundsController.Instance.ToggleMusic();
        UpdateUI();
    }

    public void ToggleSounds()
    {
        SoundsController.Instance.ToggleSFX();
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool musicOn = SoundsController.Instance.IsMusicEnabled();
        bool sfxOn = SoundsController.Instance.IsSFXEnabled();

        // music on and off active states
        musicOnSprite.gameObject.SetActive(musicOn);
        musicOffSprite.gameObject.SetActive(!musicOn);
        // sounds on and off active states
        soundsOnSprite.gameObject.SetActive(sfxOn);
        soundsOffSprite.gameObject.SetActive(!sfxOn);

    }
}