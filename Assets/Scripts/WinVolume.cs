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
    UnityEvent onSuccessCheckEvents;

    public bool playerInside = false, dadOutside = true;

    public void Start()
    {
        dadGameObject = DadBehaviour.instance.gameObject;
        playerGameObject = FindObjectOfType<PlayerController>().gameObject;
        checkVolume = GetComponent<Collider>();

    }
    public void CheckWinCondition()
    {
        if (playerInside && dadOutside)
            onSuccessCheckEvents.Invoke();
        else 
            onFailCheckEvents.Invoke();
        
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            onPlayerEnterEvents.Invoke();
            playerInside = true;
        }
        if (other.gameObject.CompareTag("Dad")) dadOutside = false;
    }
    public void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("Player")) playerInside = false;
        if(other.gameObject.CompareTag("Dad")) dadOutside = true;
    }


}
