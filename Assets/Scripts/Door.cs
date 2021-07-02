using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField]
    bool m_isLocked = false;
    
    public bool closed = true;

    private Animator animator;

    MessageSystem messageSystem;

    [SerializeField]
    public List<Key> keys;

    [SerializeField]
    Transform frontPos, backPos;

    [SerializeField]
    AnimationClip openClip = null;

    private AudioSource m_AudioSource;

    [SerializeField]
    AudioClip[] openSounds, closeSounds, lockedSounds;

    // Variaveis de tempo
    private float lastTimePressed = float.MinValue;
    public float animationClipTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator?.SetBool("IsLocked", m_isLocked);
        messageSystem = FindObjectOfType<MessageSystem>();
        if (openClip) animationClipTime = openClip.length;
        m_AudioSource = GetComponentInChildren<AudioSource>();
    }

    public Vector3 GetDockingPoint(Vector3 dadPosition, out Vector3 lookingPoint)
    {
        if(Vector3.Distance(dadPosition, frontPos.position) < Vector3.Distance(dadPosition, backPos.position))
        {
            lookingPoint = backPos.position;
            return frontPos.position;
        }
        else
        {
            lookingPoint = frontPos.position;
            return backPos.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (frontPos) Gizmos.DrawSphere(frontPos.position, 0.25f);

        Gizmos.color = Color.red;
        if (backPos) Gizmos.DrawSphere(backPos.position, 0.25f);

        Gizmos.color = Color.green;
        
        foreach (Key key in keys) 
            if(key) Gizmos.DrawLine(transform.position, key.gameObject.transform.position);

    }

    public void Pressed()
    {
        var currentTime = Time.time;

        if (currentTime < lastTimePressed + animationClipTime) 
            return; // Espera a animacao atual 

        lastTimePressed = currentTime;

        CheckForKeys();

        if (m_isLocked)
            messageSystem?.SetMessageText("A porta está trancada...", 3f);
        else
            closed = !closed;

        animator?.SetBool("Pressed", true);
        
    }

    private void CheckForKeys()
    {
        if (!m_isLocked) return;

        for (int i = keys.Count - 1; i >= 0; i--)
            if (keys[i] && keys[i].found)
            {
                Debug.Log("Destravou!");
                m_isLocked = false;
                animator?.SetBool("IsLocked", m_isLocked);
                Destroy(keys[i].gameObject);
                keys.RemoveAt(i);
            }

        
    }

    private void LockedSound() => PlaySoundAtRandom(lockedSounds);
    private void OpenSound() => PlaySoundAtRandom(openSounds);
    private void CloseSound() => PlaySoundAtRandom(closeSounds);
    

    private void PlaySoundAtRandom(AudioClip[] dictOfSounds)
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        if (dictOfSounds.Length <= 0) return;
        else if (dictOfSounds.Length == 1) m_AudioSource.PlayOneShot(dictOfSounds[0]);
        else
        {
            int n = UnityEngine.Random.Range(1, dictOfSounds.Length);

            m_AudioSource.clip = dictOfSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            dictOfSounds[n] = dictOfSounds[0];
            dictOfSounds[0] = m_AudioSource.clip;
        }
    }

    public bool DadPressed()
    {
        var currentTime = Time.time;
        if (currentTime < lastTimePressed + animationClipTime)
            return false; // Espera a animacao atual 
        lastTimePressed = currentTime;
        animator?.SetBool("DadPressed", true);
        closed = !closed;
        return true;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Key"))
            return;

        if (onlyKey is null) return;

        if (onlyKey.Equals(other.gameObject)){
            Debug.Log("Destravou!");
            m_isLocked = false;
            animator?.SetBool("IsLocked", m_isLocked);
            Destroy(other.gameObject);

            foreach (Collider collider in GetComponents<Collider>()) collider.enabled = !collider.isTrigger;

        }

    }*/

}
