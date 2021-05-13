﻿using System;
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

    public void SetAvaliableLevels(int slot = 0)
    {
        SaveGame save = SaveManager.LoadGame(slot);

        PlayerPrefs.SetInt("Slot", slot);

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
