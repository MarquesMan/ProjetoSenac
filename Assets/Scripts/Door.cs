using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField]
    bool m_isLocked = false;

    private Animator animator;

    MessageSystem messageSystem;

    [SerializeField]
    GameObject onlyKey;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator?.SetBool("IsLocked", m_isLocked);
        messageSystem = FindObjectOfType<MessageSystem>();
    }

    public void Pressed()
    {
        if (m_isLocked)
            messageSystem?.SetMessageText("A porta está trancada...", 3f);

        animator?.SetBool("Pressed", true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Key"))
            return;

        if (onlyKey is null) return;

        if (onlyKey.Equals(other.gameObject)){
            Debug.Log("Destravou!");
            m_isLocked = false;
            animator?.SetBool("IsLocked", m_isLocked);
            Destroy(other.gameObject);
        }

    }

}
