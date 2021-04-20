using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float disarmTime = 5f, currentHoldingTime = 0f;

    private GameObject trappedObject;

    void Start()
    {
 
    }

    
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.gameObject.CompareTag("Player"))
        {
            trappedObject = otherObj.gameObject;
            player.GetComponent<PlayerController>().playerStuck = true;
            Debug.LogError("Encostou");
        }
            
    }

    public void AddTime(float deltaTime)
    {
        this.currentHoldingTime += deltaTime;
        Debug.LogWarning(this.currentHoldingTime);
        if(this.currentHoldingTime >= disarmTime)
        {
            Debug.LogError("Desarmou");
            if (trappedObject != null)
                trappedObject.GetComponent<PlayerController>().playerStuck = false;
        }
    }

    public void ResetTime()
    {
        this.currentHoldingTime = 0f;
    }
}
