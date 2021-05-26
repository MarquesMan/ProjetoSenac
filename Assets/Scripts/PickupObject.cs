using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{

    public AudioClip pickupSound;
    public float pickupSpeed = 0.25f;
    internal bool found = false;

    public void PlayerPickup()
    {
        //if (playerTransform == null)
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        transform.SetParent(playerTransform, true);
        gameObject.LeanMove(playerTransform.position + playerTransform.forward * -1, pickupSpeed); ;
        found = true;
        if (pickupSound) GetComponent<AudioSource>().PlayOneShot(pickupSound);
        StartCoroutine(GoToPlayerInv());
    }

    private IEnumerator GoToPlayerInv()
    {
        yield return new WaitForSeconds(pickupSound ? pickupSound.length : pickupSpeed + 0.1f);
        gameObject.SetActive(false);
    }
}
