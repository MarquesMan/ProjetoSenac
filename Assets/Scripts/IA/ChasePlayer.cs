using UnityEngine;
using System;
using Brainiac;
using UnityEngine.AI;

[AddNodeMenu("Action/ChasePlayer")]
public class ChasePlayer : Brainiac.Action
{
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private float stoppingDistance;

    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        stoppingDistance = navMeshAgent.stoppingDistance;
        player = agent.Blackboard.GetItem<GameObject>("Player", null);        
    }

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
        // navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);

        if (navMeshAgent is null || player is null) return BehaviourNodeStatus.Failure;

        var agentPos = agent.Body.transform.position;
        agentPos.y -= 1f; // Diminui altura do player

        var distanceRay = player.transform.position - agentPos;

        if (distanceRay.magnitude < stoppingDistance) {

            return BehaviourNodeStatus.Success;
        };


        navMeshAgent.SetDestination(player.transform.position);

        return BehaviourNodeStatus.None;
	}
}