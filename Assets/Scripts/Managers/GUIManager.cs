using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject firstMenu;
    private GameObject currentMenu, oldMenu;

    public LeanTweenType  leanIn = LeanTweenType.linear,
                         leanOut = LeanTweenType.linear;

    public float fadeIn = 1f, fadeOut = 1f, waitForShowMenu = 2f;

    private void Start()
    {
        foreach (CanvasGroup canvasGroup in FindObjectsOfType<CanvasGroup>()) canvasGroup.alpha = 0f;

        FindObjectOfType<LevelManager>().levelIntro();

        if (firstMenu != null)
        {
            currentMenu = firstMenu;
            StartCoroutine(MainMenuShow());
        }

    }

    private IEnumerator MainMenuShow()
    {
        yield return new WaitForSeconds(waitForShowMenu);
        ShowMenu();        
    }

    public void ChangeCurrentMenu(GameObject menuGameObject)
    {
        if (menuGameObject == null) return;
        oldMenu = currentMenu;

        currentMenu = menuGameObject;        
        oldMenu?.GetComponent<CanvasGroup>().LeanAlpha(0f, fadeOut).setEase(leanOut).setOnComplete(ShowMenu);
    }

    private void ShowMenu()
    {
       
        if (oldMenu) oldMenu.SetActive(false);
        currentMenu.SetActive(true);
        currentMenu.GetComponent<CanvasGroup>().alpha = 0f;
        currentMenu?.GetComponent<CanvasGroup>().LeanAlpha(1f, fadeIn).setEase(leanIn);
    }
}
