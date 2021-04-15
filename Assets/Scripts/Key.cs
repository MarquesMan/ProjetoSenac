using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    // Start is called before the first frame update
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
}
