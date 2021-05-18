using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    // Start is called before the first frame update
    public AudioClip pickupSound;
    public float pickupSpeed = 0.25f;
    internal bool found = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void RestartPosition()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        var rigidBody = this.GetComponent<Rigidbody>();
        if(rigidBody!= null) rigidBody.velocity = Vector3.zero;
    }

    public void PlayerPickup(Transform playerTransform)
    {
        transform.SetParent(playerTransform, true);
        gameObject.LeanMove(playerTransform.position + playerTransform.forward*-1, pickupSpeed);;
        found = true;
        if (pickupSound) GetComponent<AudioSource>().PlayOneShot(pickupSound);
        StartCoroutine(GoToPlayerInv());        
    }

    private IEnumerator GoToPlayerInv()
    {
        yield return new WaitForSeconds(pickupSound? pickupSound.length : pickupSpeed + 0.1f);
        gameObject.SetActive(false);
    }
}
