using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
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

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        SetUpResolutions();
        SetUpFullscreen();

        SetUpMusic();
        SetUpSFX();
    }

    private void SetUpResolutions()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) currentResolutionIndex = i;
        }

        resolutionsDropdown.AddOptions(resolutionOptions);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    private void SetUpFullscreen()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    private void SetUpMusic()
    {
        musicMixer.GetFloat("Volume", out float volume);

        musicVolumeSlider.value = (volume + 80) * 100;
        ChangeMusicVolume(musicVolumeSlider.value);
    }

    private void SetUpSFX()
    {
        sfxMixer.GetFloat("Volume", out float volume);

        sfxVolumeSlider.value = (volume + 80) * 100;
        ChangeSFXVolume(sfxVolumeSlider.value);
    }

    private void ApplyResolutionAndFullscreen()
    {
        Resolution newResolution = resolutions[resolutionsDropdown.value];
        Screen.SetResolution(newResolution.width, newResolution.height, fullscreenToggle.isOn);
    }

    private void ApplyMusicVolume()
    {
        musicMixer.SetFloat("Volume", 0.8f * musicVolumeSlider.value - 80);
    }

    private void ApplySFXVolume()
    {
        sfxMixer.SetFloat("Volume", 0.8f * sfxVolumeSlider.value - 80);
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
