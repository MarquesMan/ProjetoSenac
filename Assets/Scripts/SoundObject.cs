using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > 1f)
            DadBehaviour.HearSound(this.gameObject);
    }
}
