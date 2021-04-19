using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject player;
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
            player.GetComponent<PlayerController>().playerStuck = true;
            Debug.LogError("Encostou");
        }
            
    }
}
