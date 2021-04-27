using UnityEngine;
using System;
using Brainiac;
using System.Collections.Generic;
using UnityEngine.AI;

[AddNodeMenu("Action/GoToNextPatrolPoint")]
public class GoToNextPatrolPoint : Brainiac.Action
{
    private NavMeshAgent navMeshAgent;
    private List<Vector3> patrolPoints = null;
    private Vector3 currentPoint;
    private int currentIndex = 0;
    private float minDistance = 2f;

    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        patrolPoints = agent.Blackboard.GetItem<List<Vector3>>("PatrolPoints", null);
        foreach (Vector3 vec in patrolPoints)
            Debug.LogWarning(vec);

        if (patrolPoints != null) currentPoint = patrolPoints[currentIndex];
    }
    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
        if (patrolPoints is null) return BehaviourNodeStatus.Failure;

        if (Vector3.Distance(agent.Body.transform.position, currentPoint) <= minDistance)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Count;
            currentPoint = patrolPoints[ currentIndex ];
            return BehaviourNodeStatus.Success;
        }

        navMeshAgent.SetDestination(currentPoint);
        return BehaviourNodeStatus.Running;
	}
}