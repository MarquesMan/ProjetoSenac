using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] List<GameObject> listOfSaveSlots;
    private LevelManager levelManager;
    private bool newGame = false;

    private Dictionary<string, int> dictId = new Dictionary<string, int> { { "Slot1", 0 }, { "Slot2", 1 }, { "Slot3", 2 } };
    private Dictionary<string, GameObject> slotPanelId = new Dictionary<string, GameObject> { };
    private Button currentButton = null;
    public void FillInformation()
    {
        

        for(int i =0; i < listOfSaveSlots.Count; ++i)
        {
            var saveGame = SaveManager.LoadGame(i);

            if (saveGame == null)
            { // Escreva Disponivel
                listOfSaveSlots[i].GetComponentsInChildren<Text>()[1].text = "Disponível";
                listOfSaveSlots[i].GetComponentsInChildren<Text>()[1].alignment = TextAnchor.MiddleCenter;
            }
            else // Caso contrario escreva os detalhes do save
            {                
                listOfSaveSlots[i].GetComponentsInChildren<Text>()[1].text = saveGame.FormatText();
                listOfSaveSlots[i].GetComponentsInChildren<Text>()[1].alignment = TextAnchor.MiddleRight;
            }
        }


    }

    private void Start()
    {

        for (int i = 0; i < listOfSaveSlots.Count; i++)
        {
            var button = listOfSaveSlots[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { OnSlotClicked(button); });
        }

        for(int i = 2; i < transform.childCount-1; i+=2)
        {
            slotPanelId.Add(transform.GetChild(i).gameObject.name, transform.GetChild(i + 1).gameObject);
        }

        hideAllPanels();

        levelManager = FindObjectOfType<LevelManager>();
    }

    private void hideAllPanels()
    {
        foreach (GameObject panel in slotPanelId.Values) panel.SetActive(false);
    }

    private void OnSlotClicked(Button button)
    {
        currentButton = button;

        var id = dictId[button.gameObject.name];
        bool savePresent = SaveManager.CheckForSlot(id);

        if (newGame && !savePresent)// Create new Game options
        {
            SaveManager.SaveGame(id); // Cria um novo save
            levelManager.LoadSceneWithBuildIndex(1); // Inicia um novo jogo
        }
        else
        {
            hideAllPanels();
            if (savePresent)
            {
                slotPanelId[button.gameObject.name].SetActive(true);
                slotPanelId[button.gameObject.name].transform.GetChild(0).gameObject.SetActive(!newGame);
            } 
            button.Select();
        }

    }


    public void CheckForAvaliableSlots()
    {
        newGame = true;
        FillInformation();

        for(int i = 0; i < listOfSaveSlots.Count; ++i)
        {
            var button = listOfSaveSlots[i].GetComponent<Button>();

          /*  if (button)
                button.interactable = !SaveManager.CheckForSlot(i);*/
            
        }
    }

    public void CheckForAvaliableSaves()
    {
        newGame = false;
        FillInformation();
        for (int i = 0; i < listOfSaveSlots.Count; ++i)
        {
            var button = listOfSaveSlots[i].GetComponent<Button>();

            /*if (button)
                button.interactable = SaveManager.CheckForSlot(i);*/

        }
    }

    public void CreateSaveSlot(int slot)
    {
        SaveManager.SaveGame(slot);
    }

    public void DeleteSaveGame(int slot)
    {
        SaveManager.DeleteSaveGame(slot);
        hideAllPanels();
        currentButton.Select();
        FillInformation();
    }    

}
