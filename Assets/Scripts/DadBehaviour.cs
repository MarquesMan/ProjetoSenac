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

    [Range(0f,180f)]
    [SerializeField] float fieldOfView = 45f;

    [Range(1f, 9999f)]
    [SerializeField] float viewDistance = 10f;

    private static float degreeToRad = 0.01745329252f;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + 
            RotateVectorOnYAxis(transform.forward, fieldOfView) * viewDistance);

        Gizmos.DrawLine(transform.position, transform.position +
            RotateVectorOnYAxis(transform.forward, -fieldOfView) * viewDistance);

        Gizmos.DrawLine(transform.position, transform.position +
            RotateVectorOnXAxis(transform.forward, fieldOfView) * viewDistance);

        Gizmos.DrawLine(transform.position, transform.position +
            RotateVectorOnXAxis(transform.forward, -fieldOfView) * viewDistance);
    }

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        var blackBoard = GetComponent<Brainiac.Blackboard>();
        if (blackBoard)
        {
            blackBoard.SetItem("ViewDistance", viewDistance);
            blackBoard.SetItem("FieldOfView", fieldOfView);
            blackBoard.SetItem("Doors",
                GameObject.FindGameObjectsWithTag("Door")
            );

            blackBoard.SetItem("NavMeshAgent", agent);
        }

    }

    private Vector3 RotateVectorOnYAxis(Vector3 originalVector, float angle)
    {
        /*        
        | cos θ    0   sin θ| | x |   | x cos θ +z sin θ|   | x'|
        | 0      1       0  | | y | = | y | = | y'|
        |−sin θ    0   cos θ| | z |   |−x sin θ + z cos θ|   | z'|*/
        angle *= degreeToRad;
        return new Vector3(
            originalVector.x * Mathf.Cos(angle) + originalVector.z * Mathf.Sin(angle),
            originalVector.y,
            -originalVector.x * Mathf.Sin(angle) + originalVector.z * Mathf.Cos(angle)
        );
    }

    private Vector3 RotateVectorOnXAxis(Vector3 originalVector, float angle)
    {
        /*
        | 1     0           0| | x |   | x |   | x'|
        | 0   cos θ    −sin θ| | y | = | y cos θ − z sin θ | = | y'|
        | 0   sin θ     cos θ| | z |   | y sin θ +z cos θ| = | z'|*/

        angle *= degreeToRad;
        return new Vector3(originalVector.x,
            originalVector.y * Mathf.Cos(angle) - originalVector.z * Mathf.Sin(angle),
            originalVector.y * Mathf.Sin(angle) + originalVector.z * Mathf.Cos(angle)
        );
    }

    public static void HearSound(GameObject soundOrigin)
    {
        // instance.agent.SetDestination(soundOrigin.transform.position);
        Debug.LogError("Ouvi alguma coisa");
    }

    // Update is called once per frame
    /*void Update()
    {
        if (target != null)
            agent.SetDestination(target.transform.position);
    }*/

    // Funcao dummy por enquanto
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning((transform.position - other.transform.position).magnitude);
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
        }else if (other.gameObject.CompareTag("Door"))
        {
            other.gameObject.GetComponent<Door>().Pressed();
        }        
        else if (other.gameObject.Equals(instance.target))
        {
            instance.target = null;
            instance.agent.SetDestination(instance.startPosition);
        }
    }

}
