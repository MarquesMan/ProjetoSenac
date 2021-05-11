using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject startScreen, levelsPanel;


    public bool showText, showBlackScreen;

    public void Start()
    {
        if (startScreen == null) return;




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
