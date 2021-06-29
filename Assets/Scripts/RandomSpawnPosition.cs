using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPosition : MonoBehaviour
{
    [SerializeField]
    private RandomSpawnPoints originalObject;
    
    private Mesh mesh;


    private void SetOriginalObject()
    {
        mesh = originalObject.gameObject.GetComponent<MeshFilter>().sharedMesh;
    }
    private void Awake()
    {
        if(originalObject)
            originalObject.InsertTransform(this);
    }

    void Start()
    {
        enabled = false;    
    }

    private void OnDrawGizmos()
    {
        if (mesh)
            Gizmos.DrawMesh(mesh, -1, transform.position, transform.rotation, transform.localScale);
        else if(originalObject)
            SetOriginalObject();
    }

}
