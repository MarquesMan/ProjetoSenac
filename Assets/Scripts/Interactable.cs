using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    [SerializeField]
    UnityEvent unityEvents;

    [SerializeField] public string itemDescription = "";
    
    public void Pressed()
    {
        unityEvents.Invoke();
    }
}
