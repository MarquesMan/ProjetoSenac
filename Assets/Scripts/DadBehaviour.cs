using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DadBehaviour : MonoBehaviour
{

    private NavMeshAgent agent;
    public GameObject target;

    private Vector3 startPosition;

    private static DadBehaviour dadBehaviour;

    [SerializeField] UnityEngine.UI.Image gameOverScreen;

    public static DadBehaviour instance
    {
        get
        {
            if (!dadBehaviour)
            {
                dadBehaviour = FindObjectOfType(typeof(DadBehaviour)) as DadBehaviour;

                if (!dadBehaviour)
                {
                    Debug.LogError("There needs to be one active Dad Behaviour script on a GameObject in your scene.");
                }
                /*else
                {
                    dadBehaviour.Init();
                }*/
            }

            return dadBehaviour;
        }
    }

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
    }

    public static void HearSound(GameObject soundOrigin)
    {
        instance.agent.SetDestination(soundOrigin.transform.position);
        Debug.LogError("Ouvi alguma coisa");
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            agent.SetDestination(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError($"{other.gameObject.name} entrou na vista");
        if (other.gameObject.CompareTag("Player"))
        {

            if ((transform.position - other.transform.position).magnitude <= 5f)
            {
                Debug.LogError("Matou o player");
                gameOverScreen.enabled = true;
            }

            Debug.LogError("Player a vista!");
            instance.target = other.gameObject;
        }else if (other.gameObject.Equals(instance.target))
        {
            instance.target = null;
            instance.agent.SetDestination(instance.startPosition);
        }
    }

}
