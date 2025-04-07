using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private const string SFX_VOLUME_PARAM = "SFXVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_PARAM, 0.75f);
        float savedMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 0.75f);

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
            SetSFXVolume(savedSFXVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
            SetMusicVolume(savedMusicVolume);
        }
    }

    public void NewGame(int indexLevel)
    {
        SceneManager.LoadScene(indexLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetSFXVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            mixerVolume = -80f;

        audioMixer.SetFloat(SFX_VOLUME_PARAM, mixerVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_PARAM, volume);
    }

    public void SetMusicVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            mixerVolume = -80f;

        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, mixerVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PARAM, volume);
    }
}