using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleMaterial : MonoBehaviour
{
    [SerializeField] Material newMaterial;
    [SerializeField] int materialSlot = 0;
    Material oldMaterial;
    private bool toogle = false;
    private MeshRenderer meshRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (materialSlot >= GetComponent<MeshRenderer>().materials.Length || newMaterial == null)
        {
            enabled = false;
            return;
        }
        meshRenderer = GetComponent<MeshRenderer>();
        oldMaterial = meshRenderer.materials[materialSlot];
    }

    public void Toogle()
    {

        toogle = !toogle;

        var mats = meshRenderer.materials;
        

        if (toogle) // Esta com o material novo
        {
            mats[materialSlot] = newMaterial;
        }
        else // Esta com o material velho
        {
            mats[materialSlot] = oldMaterial;
        }

        meshRenderer.materials = mats;
    }
}
