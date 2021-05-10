using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject startScreen;

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

}
