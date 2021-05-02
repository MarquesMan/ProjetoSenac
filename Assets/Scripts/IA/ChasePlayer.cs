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
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);

        if (navMeshAgent is null || player is null) return BehaviourNodeStatus.Failure;

        var distanceRay = player.transform.position - agent.Body.transform.position;
        // distanceRay.y = 0;

        if (distanceRay.magnitude < stoppingDistance) {

            return BehaviourNodeStatus.Success;
        };


        navMeshAgent.SetDestination(player.transform.position);

        return BehaviourNodeStatus.Running;
	}
}