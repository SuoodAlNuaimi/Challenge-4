using UnityEngine;

public class SoundsController : MonoBehaviour
{
    public static SoundsController Instance { get; private set; }

    private const string MUSIC_KEY = "MusicEnabled";
    private const string SFX_KEY = "SFXEnabled";

    [Header("BG Music")]
    public AudioClip BGMusic;
    public AudioSource BGMusicSource;

    [Header("SFX")]
    public AudioClip ballHitSFX;
    public AudioClip playerGoalSFX;
    public AudioClip landDownSFX;
    public AudioClip powerupSFX;
    public AudioSource sfxSource;

    private bool musicEnabled;
    private bool sfxEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
        SetupAudio();
    }

    private void LoadSettings()
    {
        musicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    private void SetupAudio()
    {
        BGMusicSource.loop = true;
        BGMusicSource.clip = BGMusic;

        if (musicEnabled)
            BGMusicSource.Play();
        else
            BGMusicSource.Stop();
    }

    //----------------------------------------------------
    // MUSIC CONTROL
    //----------------------------------------------------

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;

        PlayerPrefs.SetInt(MUSIC_KEY, musicEnabled ? 1 : 0);
        PlayerPrefs.Save();

        if (musicEnabled)
            BGMusicSource.Play();
        else
            BGMusicSource.Stop();
    }

    public bool IsMusicEnabled()
    {
        return musicEnabled;
    }

    //----------------------------------------------------
    // SFX CONTROL
    //----------------------------------------------------

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;

        PlayerPrefs.SetInt(SFX_KEY, sfxEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsSFXEnabled()
    {
        return sfxEnabled;
    }

    //----------------------------------------------------
    // PLAY SFX
    //----------------------------------------------------

    public void PlayBallHit()
    {
        if (!sfxEnabled) return;
        sfxSource.PlayOneShot(ballHitSFX);
    }

    public void PlayGoal()
    {
        if (!sfxEnabled) return;
        sfxSource.PlayOneShot(playerGoalSFX);
    }

    public void PlayLand()
    {
        if (!sfxEnabled) return;
        sfxSource.PlayOneShot(landDownSFX);
    }

    public void PlayPowerup()
    {
        if (!sfxEnabled) return;
        sfxSource.PlayOneShot(powerupSFX);
    }
}