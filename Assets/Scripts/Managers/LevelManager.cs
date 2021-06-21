using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject startScreen, levelsPanel;
    [SerializeField] AudioMixer m_audioMixer;

    public bool startOnAwake = true, showText, showBlackScreen;
    public float fadeOutTime = 1f, fadeInTime = 1f;
    public LeanTweenType blackScreenFadeOutType, blackScreenFadeInType;



    public void Start()
    {        
        if (startOnAwake) levelIntro();
    }

    private void Awake()
    {
        var selectors = FindObjectsOfType<LevelManager>();
            if (selectors.Length > 0 && selectors[0] == this) ApplyUserSettings();
    }


    public void ApplyUserSettings()
    {

        var qualityIndex = PlayerPrefs.GetInt("QualitySettingPreference", 3);

        if (qualityIndex != 6) // if the user is not using any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);
        else
        {
            QualitySettings.masterTextureLimit = PlayerPrefs.GetInt("TextureQualityPreference", 0);
            QualitySettings.antiAliasing = PlayerPrefs.GetInt("AntiAliasingPreference", 0);
        }

        var resIndex = PlayerPrefs.GetInt("ResolutionPreference", -1);

        if (resIndex >= 0)
        {
            // Get clean list of resolutions
            var options = new List<string>();
            var resolutions = new List<Resolution>();
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

            if(resIndex <= resolutions.Count)
                Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, true);

        }

        Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference", 1));

        float volume = PlayerPrefs.GetFloat("VolumePreference", 1f);

        if(m_audioMixer) m_audioMixer.SetFloat("Volume", LeanTween.easeOutCubic(-80, 0, volume));
       

    }


    public void levelIntro()
    {
        if (startScreen == null) return;

        startScreen.SetActive(true);

        var titleText = startScreen.transform.GetChild(0).gameObject;
        if (titleText && !showText)
            titleText.SetActive(false);

        if (showBlackScreen)
        {
            startScreen.GetComponent<CanvasGroup>().alpha = 1f;
            startScreen.GetComponent<CanvasGroup>().LeanAlpha(0, fadeOutTime).setEase(blackScreenFadeOutType).setOnComplete(disableStartScreen);
        }
    }

    private void disableStartScreen()
    {
        startScreen?.SetActive(false);
    }

    public void LoadSceneWithBuildIndex(int buildIndex = 0)
    {
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartDay(int sceneIndex = 1)
    {
        //startScreen.LeanAlpha(1, fadeInTime).setOnComplete(onComplete: delegate { SceneManager.LoadScene(0, LoadSceneMode.Single); });
    }

    public void LevelPassed()
    {
        var saveSlot = PlayerPrefs.GetInt("Slot", -1);
        
        Debug.Log(saveSlot);

        if (saveSlot >= 0) {
            var savegame = SaveManager.LoadGame(saveSlot);
            
            if (savegame == null) 
                savegame = SaveManager.CreateNewGame();

            savegame.levelPassed(SceneManager.GetActiveScene().buildIndex);
            SaveManager.SaveGame(saveSlot, savegame);
        } 
        
        LoadNextScene();
    }

    public void SetAvaliableLevels()
    {
        var slot = PlayerPrefs.GetInt("Slot", -1);
        if (slot < 0) return;

        SaveGame save = SaveManager.LoadGame(slot);


        GameObject template = levelsPanel.transform.GetChild(0).gameObject;
        template.SetActive(false);

        for (int i = 1; i < levelsPanel.transform.childCount; ++i)
            Destroy(levelsPanel.transform.GetChild(i).gameObject);

        Debug.Log(save.maxLevelBeated);

        var levelsPassed = (save.maxLevelBeated+1) % (SceneManager.sceneCountInBuildSettings);

        for (int i = 1; i <= levelsPassed; ++i) // Cria os botoes
        {
            var tempObject = Instantiate<GameObject>(template);
            tempObject.transform.SetParent(levelsPanel.transform);
            tempObject.GetComponentInChildren<TMPro.TextMeshProUGUI>()?.SetText(
                $"Dia {i}"
            );
            var new_i = i;
            tempObject.GetComponent<Button>().onClick.AddListener( delegate { LoadSceneWithBuildIndex(new_i); } );
            tempObject.SetActive(true);
        }

    }

}
