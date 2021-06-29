using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;
    public TMPro.TMP_Dropdown textureDropdown;
    public TMPro.TMP_Dropdown aaDropdown;
    public TMPro.TMP_Dropdown waterDropdown;
    public Slider volumeSlider;

    float currentVolume;
    private List<string> options;
    List<Resolution> resolutions;
    private int currentResolutionIndex;

    private GUIManager guiManager;

    // Start is called before the first frame update
    void Start()
    {

        resolutionDropdown.ClearOptions();
        GetResolutionOptions();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
        guiManager = FindObjectOfType<GUIManager>();
    }

    public void ChangeCurrentMenuInterface(GameObject gameObject)
    {
        guiManager?.ChangeCurrentMenu(gameObject);
    }

    private void GetResolutionOptions()
    {
        options = new List<string>();
        resolutions = new List<Resolution>();

        currentResolutionIndex = 0;

        var tempResArray = Screen.resolutions;

        for (int i = 0; i < tempResArray.Length; i++)
        {
            string option = tempResArray[i].width + " x " + tempResArray[i].height;
            if ((tempResArray[i].width >= 640 && tempResArray[i].height >= 480) && !options.Contains(option))
            {
                options.Add(option);
                resolutions.Add(tempResArray[i]);
            }
        }

        for (int i = 0; i < resolutions.Count; ++i)
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;

    }

    public void SetVolume(float volume)
    {       
        audioMixer.SetFloat("Volume", LeanTween.easeOutCubic(-80, 0, volume));
        currentVolume = volume;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    public void SetWaterQuality(int qualityIndex)
    {
        waterDropdown.value = qualityIndex;
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);

        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }
        
        qualityDropdown.value = qualityIndex;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 0;

        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            aaDropdown.value = 0;

        waterDropdown.value = PlayerPrefs.GetInt("WaterQuality", 2);

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("VolumePreference"))
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        else
            volumeSlider.value = 1f;

        SetVolume(volumeSlider.value);
    }
}
