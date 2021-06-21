using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    private AudioSource m_AudioSource;

    [SerializeField]
    AudioClip[] footstepSounds;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void PlayFootstepSound()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        if (footstepSounds.Length <= 0) return;
        else if (footstepSounds.Length == 1) m_AudioSource.PlayOneShot(footstepSounds[0]);
        else
        {
            int n = UnityEngine.Random.Range(1, footstepSounds.Length);

            m_AudioSource.clip = footstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            footstepSounds[n] = footstepSounds[0];
            footstepSounds[0] = m_AudioSource.clip;
        }
    }
}
