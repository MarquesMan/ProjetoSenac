using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] float disarmTime = 5f, currentHoldingTime = 0f;

    private GameObject trappedObject;
    private Animator m_animator;

    private Collider trapCollider;

    private bool opening = false,
                 disarmed = false;   

    void Start()
    {
        m_animator = GetComponent<Animator>();
        trapCollider = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider otherObj)
    {
        trappedObject = otherObj.gameObject;
        
        if (trappedObject.CompareTag("Player"))
        {
            trappedObject.GetComponent<PlayerController>().playerStuck = true;
           
            trappedObject.transform.position = new Vector3(
            transform.position.x,
            trappedObject.transform.position.y,
            transform.position.z
            );

            DadBehaviour.HearSound(this.gameObject);
            m_animator.SetBool("Triggered", true);
        }
    }

    public void AddTime(float deltaTime)
    {
        this.currentHoldingTime += deltaTime;

        if (opening == false)
        {
            opening = true;
        }

        if(this.currentHoldingTime >= disarmTime)
        {
            if (trappedObject != null)
                trappedObject.GetComponent<PlayerController>().playerStuck = false;

            m_animator.SetBool("Triggered", false);
            trapCollider.enabled = false;
            disarmed = true;
        }

        m_animator.SetFloat("OpenTime", Mathf.Clamp(currentHoldingTime/disarmTime, 0, 1.1f) );

    }

    public void ResetTime()
    {
        opening = false;
        if(!disarmed) StartCoroutine(ClosingTime());
    }

    private IEnumerator ClosingTime()
    {
        while (currentHoldingTime >= 0 && !opening && !disarmed)
        {
            yield return new WaitForEndOfFrame();
            currentHoldingTime -= 15f * Time.deltaTime;
            m_animator.SetFloat("OpenTime", Mathf.Clamp(currentHoldingTime / disarmTime, 0, 1.1f));
        }
    }
}
