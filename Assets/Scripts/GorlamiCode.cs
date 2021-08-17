using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GorlamiCode : MonoBehaviour
{
    KeyCode[] correctCode = new KeyCode[] { 
        KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow,
        KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A 
    };

    private int currentCodeIndex = 0;

    [SerializeField] GameObject levelsPanel;

    // Start is called before the first frame update
    bool checkCodeIndex(KeyCode currentKey)
    {
        if (currentCodeIndex < correctCode.Length) {
            if(correctCode[currentCodeIndex] == currentKey) ++currentCodeIndex;
            else currentCodeIndex = 0;
        }  
        return currentCodeIndex >= correctCode.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKeyDown) return;

        KeyCode tempKey = KeyCode.Delete;

        if (Input.GetKeyDown(KeyCode.B)) tempKey = KeyCode.B;
        else if (Input.GetKeyDown(KeyCode.A)) tempKey = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) tempKey = KeyCode.UpArrow;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) tempKey = KeyCode.DownArrow;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) tempKey = KeyCode.LeftArrow;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) tempKey = KeyCode.RightArrow;

        if (tempKey != KeyCode.Delete)
        {
            if (checkCodeIndex(tempKey)) {

                foreach (Button button in levelsPanel.GetComponentsInChildren<Button>())
                    button.enabled = true;
            }
            // Debug.Log(currentCodeIndex);
        }

    }
}
