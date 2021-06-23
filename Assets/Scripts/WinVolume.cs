using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinVolume : MonoBehaviour
{
    GameObject dadGameObject, playerGameObject;

    Collider checkVolume;

    [SerializeField]
    UnityEvent onPlayerEnterEvents;

    [SerializeField]
    UnityEvent onFailCheckEvents;

    [SerializeField]
    UnityEvent onMissingKeyEvents;

    [SerializeField]
    UnityEvent onSuccessCheckEvents;

    [SerializeField]
    GameObject winDoor;

    private Door winDoorComponent;
    private Key winDoorKey = null;

    public bool playerInside = false, dadOutside = true;

    public void Start()
    {
        dadGameObject = DadBehaviour.instance.gameObject;
        playerGameObject = FindObjectOfType<PlayerController>().gameObject;
        checkVolume = GetComponent<Collider>();
        winDoorComponent = winDoor.GetComponent<Door>();

        if(winDoorComponent.keys.Count > 0)
            winDoorKey = winDoorComponent.keys[0];

    }
    public void CheckWinCondition()
    {
        if (!playerInside)
        {
            winDoor.GetComponent<Interactable>()?.SetItemDescr("");
            winDoorComponent.Pressed();
            return;
        }

        if(!winDoorComponent.closed)
        {
            winDoorComponent.Pressed();
            winDoor.GetComponent<Interactable>()?.SetItemDescr("Trancar");
            return;
        }

        var hasKey = winDoorKey ? winDoorKey.found : false;
        if (dadOutside && hasKey)
            onSuccessCheckEvents.Invoke();
        else if (!hasKey)
            onMissingKeyEvents.Invoke();
        else
            onFailCheckEvents.Invoke();

    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            onPlayerEnterEvents.Invoke();
            playerInside = true;
        }
        if (other.gameObject.CompareTag("Dad")) dadOutside = false;
    }
    public void OnTriggerExit(Collider other)
    {
        
        if(other.gameObject.CompareTag("Player")) playerInside = false;
        if(other.gameObject.CompareTag("Dad")) dadOutside = true;
    }


}
