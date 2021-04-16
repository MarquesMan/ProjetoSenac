using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        DadBehaviour.HearSound(this.gameObject);
    }
}
