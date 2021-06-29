using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{

    [SerializeField]
    private Vector2 timeBetweenRange;

    [SerializeField]
    AudioClip[] sounds;
    private AudioSource m_AudioSource;


    // Start is called before the first frame update
    void Start()
    {

        m_AudioSource = GetComponent<AudioSource>();
        StartCoroutine(SoundsRoutine());
    }

    // Update is called once per frame
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator SoundsRoutine()
    {
        
        int n = UnityEngine.Random.Range(1, sounds.Length);
        m_AudioSource.clip = sounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        sounds[n] = sounds[0];
        sounds[0] = m_AudioSource.clip;

        yield return new WaitForSeconds(UnityEngine.Random.Range(timeBetweenRange.x, timeBetweenRange.y) + m_AudioSource.clip.length);        
        StartCoroutine(SoundsRoutine());
        
    }

}
