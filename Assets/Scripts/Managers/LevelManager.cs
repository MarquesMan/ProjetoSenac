using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject startScreen, levelsPanel;


    public bool startOnAwake = true, showText, showBlackScreen;
    public float fadeOutTime = 1f, fadeInTime = 1f;
    public LeanTweenType blackScreenFadeOutType, blackScreenFadeInType;

    public void Start()
    {
        if (startOnAwake) levelIntro();

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
            startScreen.GetComponent<CanvasGroup>().LeanAlpha(0, fadeOutTime).setEase(blackScreenFadeOutType);
        }
    }

    public void LoadSceneWithBuildIndex(int buildIndex = 0)
    {
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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
        startScreen.LeanAlpha(1, fadeInTime).setOnComplete(onComplete: delegate { SceneManager.LoadScene(0, LoadSceneMode.Single); });
    }

    public void SetAvaliableLevels(int slot = 0)
    {
        SaveGame save = SaveManager.LoadGame(slot);

        GameObject template = levelsPanel.transform.GetChild(0).gameObject;
        template.SetActive(false);

        for(int i = 1; i < SceneManager.sceneCountInBuildSettings; ++i) // Cria os botoes
        {
            var tempObject = Instantiate<GameObject>(template);
            tempObject.transform.SetParent(levelsPanel.transform);
            tempObject.SetActive(true);
        }

    }

}
