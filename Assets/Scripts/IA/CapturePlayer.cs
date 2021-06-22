using UnityEngine;
using System;
using Brainiac;
using Assets.IA;
using Brainiac.Serialization;
using UnityEngine.AI;

[AddNodeMenu("Action/CapturePlayer")]
public class CapturePlayer : Brainiac.Action
{

    private GameObject player;
    private bool started = false,
                 isLookingAtDad = false;

    private Vector3 playerStartPos, lookAt;
    private Camera m_camera;

    private float grabTime = 7f,
                  grabCount = 0f;

    private Transform handTransform, eyeTransform;

    public override void OnStart(AIAgent agent)
    {
        player = agent.Blackboard.GetItem<GameObject>("Player", null);
    }
    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
		if(started == false)
        {
            started = true;
            handTransform = DadBehaviour.CapturePlayer(out eyeTransform);
            DadBehaviour.ClearSoundList();
            agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null)?.SetDestination(agent.Body.transform.position);
            player.GetComponent<PlayerController>()?.DeclareGameOver(); // Trava o player
            playerStartPos = player.transform.position;
            m_camera = Camera.main;
            if (eyeTransform)
                lookAt = (eyeTransform.position - m_camera.transform.position).normalized;
            else
                lookAt = (agent.Body.transform.position - m_camera.transform.position).normalized;
        }        

        if (isLookingAtDad)
        {
            // Depois disso, levantar o player para 
            // simbolizar ele sendo levado pelo colarinho.
            if (grabCount >= grabTime) {
                player.GetComponent<PlayerController>()?.startGameOverScreen();
                return BehaviourNodeStatus.Running; 
            }
            if (eyeTransform) m_camera.transform.LookAt(eyeTransform, Vector3.up);

            m_camera.transform.position = handTransform.position; //;Vector3.Lerp(playerStartPos, playerStartPos + Vector3.up*2.5f, grabCount/grabTime );
            grabCount += Time.deltaTime;
        }
        else // Fazer o player olhar para inimigo
        {
            // Rotacionar o vetor frente da camera para
            // O vetor de distancia normalizado
            m_camera.transform.rotation = Quaternion.LookRotation(
                Vector3.RotateTowards(m_camera.transform.forward, lookAt, 180 * Utils.degreeToRad * Time.deltaTime, 0f)
            );

            isLookingAtDad = Vector3.Angle(m_camera.transform.forward, lookAt) <= 2.5f; 
        }

        return BehaviourNodeStatus.Running;

    }
}