using Brainiac;
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
    private Blackboard blackBoard;
    private static DadBehaviour dadBehaviour;

    [SerializeField] UnityEngine.UI.Image gameOverScreen;

    [Range(0f,180f)]
    [SerializeField] float fieldOfView = 45f;

    [Range(1f, 9999f)]
    [SerializeField] float viewDistance = 10f;

    [SerializeField] GameObject patrolGameObject = null;

    private static float degreeToRad = 0.01745329252f;

    private Stack<Vector3> listOfSounds = null; 

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
        blackBoard = GetComponent<Brainiac.Blackboard>();
        if (blackBoard)
        {
            blackBoard.SetItem("ViewDistance", viewDistance);
            blackBoard.SetItem("FieldOfView", fieldOfView);
            blackBoard.SetItem("Doors",
                GameObject.FindGameObjectsWithTag("Door")
            );

            blackBoard.SetItem("NavMeshAgent", agent);
            blackBoard.SetItem("ListOfSounds", new Stack<Vector3>());
            listOfSounds = blackBoard.GetItem<Stack<Vector3>>("ListOfSounds", null);

            if (patrolGameObject != null)
            {
                var listOfPatrolPoints = new List<Vector3>();
                foreach(Transform childTransform in patrolGameObject.transform)
                {
                        listOfPatrolPoints.Add(childTransform.position);
                }
                blackBoard.SetItem("PatrolPoints", listOfPatrolPoints);
            }

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
        Vector3 soundPos = soundOrigin.transform.position;
        soundPos.y = instance.transform.position.y;
        // Empilhar os sons
        instance.listOfSounds.Push(soundPos);

    }

}
