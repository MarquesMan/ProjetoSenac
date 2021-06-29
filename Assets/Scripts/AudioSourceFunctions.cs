using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceFunctions : MonoBehaviour
{


    [SerializeField]
    UnityEngine.Events.UnityEvent eventWhenFinished;

    private AudioSource audioSource;

    public float FadeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource.playOnAwake) StartCoroutine(_FadeIn());
    }

    public void Play()
    {
        audioSource.Play();
        if (audioSource.clip == null) return;
        StartCoroutine(_InvokeWhenFinished(audioSource.clip.length));
    }

    public IEnumerator _InvokeWhenFinished(float clipTime)
    {
        yield return new WaitForSeconds(clipTime);
        eventWhenFinished.Invoke();
    }

    public void FadeOut(float newVolume)
    {
        StartCoroutine(_FadeOut(FadeTime));
    }

    public IEnumerator _FadeOut(float newVolume)
    {
        float startVolume = this.audioSource.volume;

        while (audioSource.volume > newVolume)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        // audioSource.Stop();
        // audioSource.volume = startVolume;
    }

    public IEnumerator _FadeIn()
    {
        float startVolume = 0.1f;
        audioSource.volume = startVolume;

        while (audioSource.volume <= 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        // audioSource.Stop();
        // audioSource.volume = startVolume;
    }

}
