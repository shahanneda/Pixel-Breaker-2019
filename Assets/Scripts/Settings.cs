using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public SettingsFile settingsFile;
    private bool settingsFileExists = false;

    public GameObject[] menus;

    public Resolution[] resolutions;
    public Dropdown resolutionsDropdown;

    public Toggle fullscreenToggle;

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public Text musicVolumeText;
    public Slider musicVolumeSlider;

    public Text sfxVolumeText;
    public Slider sfxVolumeSlider;

    public Toggle showCardTextToggle;

    public float minimumVolume = 40f;

    private string filePath;

    private const string playerPrefReseloutionWidthStringString = "s_res_w";
    private const string playerPrefReseloutionHeightStringString = "s_res_h";
    private const string playerPrefMusicVolumeString = "s_mv";
    private const string playerPrefSfxVolString = "s_sfxv";
    private const string playerPrefFullscreenString= "s_fullscreen";
    private const string playerPrefShowText = "s_showT";

    private void Awake()
    {
        StartCoroutine(AwakeAudioSources(FindObjectsOfType<AudioSource>()));
    }

    private IEnumerator AwakeAudioSources(AudioSource[] audioSources)
    {
        foreach(AudioSource source in audioSources)
        {
            source.enabled = false;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (AudioSource source in audioSources)
        {
            source.enabled = true;
        }
    }

    private void Start()
    {
        filePath = Application.persistentDataPath + "/Settings.json";

        SetUp();
    }

    private void SetUp()
    {
        LoadSettings();

        SetUpResolutions();
        SetUpFullscreen();

        SetUpMusic();
        SetUpSFX();

        SetUpCardText();

        if (settingsFileExists)
        {
            ApplySettings();
        }
    }

    private void SetUpResolutions()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        foreach (Resolution resolution in resolutions)
        {
            resolutionOptions.Add(resolution.width + " x " + resolution.height);
        }

        resolutionsDropdown.AddOptions(resolutionOptions);

        if (!settingsFileExists)
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) currentResolutionIndex = i;
            }
        }
        else
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == settingsFile.width && resolutions[i].height == settingsFile.height) currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    private void SetUpFullscreen()
    {
        if (!settingsFileExists)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
        else
        {
            fullscreenToggle.isOn = settingsFile.fullscreen;
        }
    }

    private void SetUpMusic()
    {
        if (!settingsFileExists)
        {
            musicMixer.GetFloat("Volume", out float volume);

            musicVolumeSlider.value = (volume + minimumVolume) * 100;
            ChangeMusicVolume(musicVolumeSlider.value);
        }
        else
        {
            musicVolumeSlider.value = settingsFile.musicVolume;
        }
    }

    private void SetUpSFX()
    {
        if (!settingsFileExists)
        {
            sfxMixer.GetFloat("Volume", out float volume);

            sfxVolumeSlider.value = (volume + minimumVolume) * 100;
            ChangeSFXVolume(sfxVolumeSlider.value);
        }
        else
        {
            sfxVolumeSlider.value = settingsFile.sfxVolume;
        }
    }

    private void SetUpCardText()
    {
        if (!settingsFileExists)
        {
            showCardTextToggle.isOn = false;
        }
        else
        {
            showCardTextToggle.isOn = settingsFile.showCardText;
        }
    }

    private void ApplyResolutionAndFullscreen()
    {
        Resolution newResolution = resolutions[resolutionsDropdown.value];
        Screen.SetResolution(newResolution.width, newResolution.height, fullscreenToggle.isOn);
    }

    private void ApplyMusicVolume()
    {
        musicMixer.SetFloat("Volume", (musicVolumeSlider.value > 0) ? minimumVolume * (musicVolumeSlider.value - 100f) / 100f : -100f);
    }

    private void ApplySFXVolume()
    {
        sfxMixer.SetFloat("Volume", (sfxVolumeSlider.value > 0) ? minimumVolume * (sfxVolumeSlider.value - 100f) / 100f : -100f);
    }

    public void SaveSettings()
    {
        settingsFile = new SettingsFile();
        settingsFile.SetSettings(resolutions[resolutionsDropdown.value], fullscreenToggle.isOn, (int)musicVolumeSlider.value, (int)sfxVolumeSlider.value, showCardTextToggle.isOn);
        PlayerPrefs.SetInt(playerPrefReseloutionWidthStringString, settingsFile.width);
        PlayerPrefs.SetInt(playerPrefReseloutionHeightStringString, settingsFile.height);
        PlayerPrefs.SetInt(playerPrefMusicVolumeString, settingsFile.musicVolume);
        PlayerPrefs.SetInt(playerPrefSfxVolString, settingsFile.sfxVolume);
        PlayerPrefs.SetInt(playerPrefFullscreenString,settingsFile.fullscreen == true ? 1 : 0);
        PlayerPrefs.SetInt(playerPrefShowText, settingsFile.showCardText == true ? 1 : 0);
        //string json = JsonUtility.ToJson(settingsFile);
        //File.WriteAllText(filePath, json);
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(playerPrefMusicVolumeString))
        {

            //string json = File.ReadAllText(filePath);
            //settingsFile = JsonUtility.FromJson<SettingsFile>(json);
            settingsFile = new SettingsFile();
            settingsFile.SetSettings(
                PlayerPrefs.GetInt(playerPrefReseloutionWidthStringString),
                PlayerPrefs.GetInt(playerPrefReseloutionHeightStringString),
                PlayerPrefs.GetInt(playerPrefFullscreenString) == 1 ? true : false,
                PlayerPrefs.GetInt(playerPrefMusicVolumeString),
                PlayerPrefs.GetInt(playerPrefSfxVolString),
                PlayerPrefs.GetInt(playerPrefShowText) == 1 ? true : false
            );
            settingsFileExists = true;
        }
    }

    public void ChangeMusicVolume(float volume)
    {
        musicVolumeText.text = "MUSIC VOLUME: " + (int)volume + "%";
    }

    public void ChangeSFXVolume(float volume)
    {
        sfxVolumeText.text = "SOUNDS VOLUME: " + (int)volume + "%";
    }
     
    public void ApplySettings()
    {
        ApplyResolutionAndFullscreen();

        ApplyMusicVolume();
        ApplySFXVolume();
    }

    public void OpenMenu(GameObject menu)
    {
        foreach (GameObject m in menus)
        {
            if (m.Equals(menu)) m.SetActive(true);
            else m.SetActive(false);
        }
    }
}

[System.Serializable]
public class SettingsFile
{
    public int width;
    public int height;
    public bool fullscreen;

    public int musicVolume;
    public int sfxVolume;

    public bool showCardText;

    public SettingsFile()
    {

    }

    public SettingsFile(int width, int height, bool fullscreen, int musicVolume, int sfxVolume, bool showCardText)
    {
        this.width = width;
        this.height = height;
        this.fullscreen = fullscreen;

        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;

        this.showCardText = showCardText;
    }

    public void SetSettings(Resolution resolution, bool fullscreen, int musicVolume, int sfxVolume, bool showCardText)
    {
        width = resolution.width;
        height = resolution.height;
        this.fullscreen = fullscreen;

        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;

        this.showCardText = showCardText;
    }
    public void SetSettings(int width, int height, bool fullscreen, int musicVolume, int sfxVolume, bool showCardText)
    {
        this.width = width;
        this.height = height;
        this.fullscreen = fullscreen;

        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;

        this.showCardText = showCardText;
    }
}
