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
    GameObject onlyKey;

    [SerializeField]
    Transform frontPos, backPos;

    [SerializeField]
    AnimationClip openClip = null;

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
    }

    public void Pressed()
    {
        var currentTime = Time.time;

        if (currentTime < lastTimePressed + animationClipTime) 
            return; // Espera a animacao atual 

        lastTimePressed = currentTime;

        if (m_isLocked)
            messageSystem?.SetMessageText("A porta está trancada...", 3f);

        animator?.SetBool("Pressed", true);
        closed = !closed;
    }

    public void DadPressed()
    {
        var currentTime = Time.time;
        if (currentTime < lastTimePressed + animationClipTime)
            return; // Espera a animacao atual 
        lastTimePressed = currentTime;
        animator?.SetBool("DadPressed", true);
        closed = !closed;
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

            foreach (Collider collider in GetComponents<Collider>()) collider.enabled = !collider.isTrigger;

        }

    }

}
