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

    [BTProperty("Min Distance")]
    private float minDistance = 2f;

    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        startPos = agent.Body.transform.position;

    }

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{

        if (Utils.GameObjectInView(
            agent.Blackboard.GetItem<GameObject>("Player", null), agent.Body,
            agent.Blackboard.GetItem<float>("FieldOfView", 0f),
            agent.Blackboard.GetItem<float>("ViewDistance", 0f)
        ))
            return BehaviourNodeStatus.Success;

        if (Vector3.Distance(agent.Body.transform.position, startPos) <= minDistance)
            return BehaviourNodeStatus.Success;

        navMeshAgent.SetDestination(startPos);
        return BehaviourNodeStatus.Running;
	}
}