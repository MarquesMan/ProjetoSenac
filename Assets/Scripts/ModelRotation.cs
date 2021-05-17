using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    public Vector3 axis = Vector3.zero;
    public float angle = 0f;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(
        axis,
        angle
        );        
    }
}
