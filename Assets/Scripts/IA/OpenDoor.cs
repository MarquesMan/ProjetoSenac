using UnityEngine;
using System;
using Brainiac;
using UnityEngine.AI;

[AddNodeMenu("Action/OpenDoor")]
public class OpenDoor : Brainiac.Action
{

	private Door door;
    private Vector3 dockingPos, lookingPos;
    private bool started = false;

    private NavMeshAgent navMeshAgent;
    private float stoppingDistance, 
            waitTime = -1, waitCount = 0f;

    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
    }

    protected override void OnEnter(AIAgent agent)
    {
        door = agent.Blackboard.GetItem<Door>("Door", null);
        stoppingDistance = navMeshAgent.stoppingDistance;
        navMeshAgent.stoppingDistance = 0.5f;
    }

    protected override void OnExit(AIAgent agent)
    {
        navMeshAgent.stoppingDistance = stoppingDistance;
        waitCount = 0;
        waitTime = -1;
        door = null;
        started = false;
    }

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
        if (door == null) return BehaviourNodeStatus.Failure;

        if (waitTime > 0) // Esperar
        {
            if(waitCount >= waitTime)// Porta abriu
            {
                
                if (Vector3.Distance(agent.Body.transform.position, lookingPos) <= navMeshAgent.stoppingDistance)
                {
                    // Fechar
                    door.DadPressed(); // Abrir a porta
                    return BehaviourNodeStatus.Success;
                }
                else
                {
                    // Me direcionar ao ponto de saida da porta
                    navMeshAgent.SetDestination(lookingPos);
                }
            }

            waitCount += Time.deltaTime;            
        }
        else if (started) // Ir ao ponto de entrada da porta
        {
            navMeshAgent.SetDestination(dockingPos);
            agent.Body.transform.LookAt(lookingPos);

            if (Vector3.Distance(agent.Body.transform.position, dockingPos) <= navMeshAgent.stoppingDistance &&
                    Vector3.Angle(agent.Body.transform.forward,
                                   (lookingPos - agent.Body.transform.position).normalized) == 0)
            {
                if(door.DadPressed()) // Abrir a porta
                    waitTime = door.animationClipTime;               
            }
        }
        else
        {
            dockingPos = door.GetDockingPoint(agent.transform.position, out lookingPos);
            dockingPos.y = agent.Body.transform.position.y;
            lookingPos.y = agent.Body.transform.position.y;
            started = true;
        }
        

        


        return BehaviourNodeStatus.Running;
	}
}