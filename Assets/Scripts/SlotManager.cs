using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] List<GameObject> listOfSaveSlots;

    
    public void CheckForAvaliableSlots()
    {
        for(int i = 0; i < listOfSaveSlots.Count; ++i)
        {
            var button = listOfSaveSlots[i].GetComponent<Button>();

            if (button)
                button.interactable = !SaveManager.CheckForSlot(i);
            
        }
    }

    public void CheckForAvaliableSaves()
    {
        for (int i = 0; i < listOfSaveSlots.Count; ++i)
        {
            var button = listOfSaveSlots[i].GetComponent<Button>();

            if (button)
                button.interactable = SaveManager.CheckForSlot(i);

        }
    }

    public void CreateSaveSlot(int slot)
    {
        SaveManager.SaveGame(slot);
    }

}
