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

    [Range(1f, 200f)]
    [SerializeField] float viewDistance = 10f;

    [SerializeField]
    Transform eyeTransform = null;

    [Range(0, 1)]
    [SerializeField]
    float patrolChance = 0.5f;

    [SerializeField] GameObject patrolGameObject = null;

    private static float degreeToRad = 0.01745329252f;

    [SerializeField] float m_GroundCheckDistance = 0.1f;

    [SerializeField]
    private AudioClip gameOverSound;

    private Door lastDoorOpened;
    private float lastTimeOpened; 

    public static Transform CapturePlayer(out Transform eyePositionTransform)
    {

        instance.m_animator.SetTrigger("Capture");
        
        var randomSounds = instance.GetComponent<RandomSounds>();
        if(randomSounds) randomSounds.enabled = false;

        var audioSrc = instance.GetComponent<AudioSource>();
        if (audioSrc) audioSrc.PlayOneShot(instance.gameOverSound);

        eyePositionTransform = instance.eyeTransform;
        return instance.handTransform;
    }

    private Stack<Vector3> listOfSounds = null;

    private Animator m_animator;
    
    public Transform handTransform;
    public float isThisDoorOkTime = 1f;

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
        m_animator = GetComponentInChildren<Animator>();        

        if (blackBoard)
        {
            blackBoard.SetItem("ViewDistance", viewDistance);
            blackBoard.SetItem("FieldOfView", fieldOfView);
            blackBoard.SetItem("Doors",
                GameObject.FindGameObjectsWithTag("Door")
            );

            blackBoard.SetItem("NavMeshAgent", agent);
            blackBoard.SetItem("ListOfSounds", new Stack<Vector3>());
            
            blackBoard.SetItem("PatrolChance", patrolChance);

            listOfSounds = blackBoard.GetItem<Stack<Vector3>>("ListOfSounds", null);

            blackBoard.SetItem("Animator", m_animator);

            if (patrolGameObject != null)
            {
                var listOfPatrolPoints = new List<Vector3>();
                foreach(Transform childTransform in patrolGameObject.transform)
                {
                       if (childTransform.gameObject.activeSelf) listOfPatrolPoints.Add(childTransform.position);
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
        // soundPos.y = instance.transform.position.y;
        // Empilhar os sons

        NavMeshHit myNavHit;

        if (NavMesh.SamplePosition(soundPos, out myNavHit, 100, -1))
        {
            if (instance.listOfSounds != null)
                instance.listOfSounds.Push(myNavHit.position);
        }
    }

    public static void ClearSoundList() => instance.listOfSounds.Clear();

    private void Update()
    {
        var move = agent.velocity;
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);


        RaycastHit hitInfo;

        var m_GroundNormal = Vector3.up;

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            //m_IsGrounded = true;
            //m_Animator.applyRootMotion = true;
        }

        move = Vector3.ProjectOnPlane(move, m_GroundNormal);       

        m_animator.SetFloat("Forward", move.z, 0.1f, Time.deltaTime );
    }

    public static bool canDadOpenThisDoor(Door currentDoor)
    {
        float currentTime = Time.time;
        
        if ( instance.lastDoorOpened == null || !instance.lastDoorOpened.Equals(currentDoor))
        {
            
            instance.lastDoorOpened = currentDoor;
            instance.lastTimeOpened = currentTime;
            return true;
        }
        else
        {
            
            if((currentTime - instance.lastTimeOpened) >= instance.isThisDoorOkTime)
            {
                instance.lastTimeOpened = currentTime;
                instance.lastDoorOpened = currentDoor;
                
                return true;
            }

            return false;
        }
    }


}
