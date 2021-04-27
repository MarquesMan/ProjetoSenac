using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 

public class SoundObject : MonoBehaviour
{
    [SerializeField] private AudioClip[] listOfSounds;

    private AudioSource m_AudioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > 1f)
        {
            PlayRandomSound();
            if(!collision.collider.CompareTag("Dad"))
                DadBehaviour.HearSound(this.gameObject);
        }
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void PlayRandomSound()
    {
        if (listOfSounds.Length == 0) return; // Caso tenha nenhum som
        else if(listOfSounds.Length == 1) m_AudioSource.clip = listOfSounds[0]; // Caso tenha so um som
        else // Caso tenha mais de um som
        {
            int n = Random.Range(1, listOfSounds.Length);
            m_AudioSource.clip = listOfSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);

            // move picked sound to index 0 so it's not picked next time
            listOfSounds[n] = listOfSounds[0];
            listOfSounds[0] = m_AudioSource.clip;
        }

    }

}
