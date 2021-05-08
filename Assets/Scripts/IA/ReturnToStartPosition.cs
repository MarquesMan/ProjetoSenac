using UnityEngine;
using System;
using Brainiac;
using UnityEngine.AI;
using Brainiac.Serialization;
using Assets.IA;

[AddNodeMenu("Action/ReturnToStartPosition")]
public class ReturnToStartPosition : Brainiac.Action
{

    private NavMeshAgent navMeshAgent;

    private Vector3 startPos = Vector3.zero;

    private float minDistance = 2f;
    private float patrolChance = 0.5f;

    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        minDistance = navMeshAgent.stoppingDistance;

        startPos = agent.Body.transform.position;
        patrolChance = agent.Blackboard.GetItem<float>("PatrolChance", 0.5f);
    }

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{

        if (agent.Blackboard.GetItem<bool>("ShouldPatrol", false)) return BehaviourNodeStatus.Failure;

        if (Vector3.Distance(agent.Body.transform.position, startPos) <= minDistance)
        {
            agent.Blackboard.SetItem("ShouldPatrol", UnityEngine.Random.value <= patrolChance);
            return BehaviourNodeStatus.Success;
        }

        navMeshAgent.SetDestination(startPos);
        return BehaviourNodeStatus.None;
	}
}