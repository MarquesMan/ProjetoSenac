using UnityEngine;
using System;
using Brainiac;
using System.Collections.Generic;
using UnityEngine.AI;
using Assets.IA;

[AddNodeMenu("Action/GoToNextPatrolPoint")]
public class GoToNextPatrolPoint : Brainiac.Action
{
    private NavMeshAgent navMeshAgent;
    private List<Vector3> patrolPoints = null;
    private Vector3 currentPoint;
    private int currentIndex = 0;
    private float minDistance = 2f;
    private Stack<Vector3> listOfSounds;


    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        minDistance = navMeshAgent.stoppingDistance;
        patrolPoints = agent.Blackboard.GetItem<List<Vector3>>("PatrolPoints", null);
        listOfSounds = agent.Blackboard.GetItem<Stack<Vector3>>("ListOfSounds", null);
        ChooseFirstLocation(agent.transform.position);
    }

    private void ChooseFirstLocation(Vector3 agentPosition)
    {
        // Caso a lista nao exista ou esteja vazia
        if (patrolPoints == null || patrolPoints.Count <= 0 ) return;
        
        // Assuma que a menor distancia eh o primeiro ponto
        currentIndex = 0;
        var minDistance = (patrolPoints[currentIndex] - agentPosition).magnitude;
        currentPoint = patrolPoints[currentIndex];

        // Percorrendo a lista de 0 ate n-1
        for (int i = 1; i < patrolPoints.Count; ++i)
        {
            // Nova distancia = distancia do pai ate o ponto na posicao i
            var newDistance = (patrolPoints[i] - agentPosition).magnitude;
            if (newDistance < minDistance)
            {
                minDistance = newDistance;
                currentIndex = i;
                currentPoint = patrolPoints[currentIndex];
            }
        }


    }

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
        if (patrolPoints == null) return BehaviourNodeStatus.Failure;

        if (listOfSounds != null && listOfSounds.Count > 0) return BehaviourNodeStatus.None;

        /*if (Utils.GameObjectInView(
            agent.Blackboard.GetItem<GameObject>("Player", null), agent.Body,
            agent.Blackboard.GetItem<float>("FieldOfView", 0f),
            agent.Blackboard.GetItem<float>("ViewDistance", 0f)
        ))
            return BehaviourNodeStatus.Success;*/


        if (Vector3.Distance(agent.Body.transform.position, currentPoint) <= minDistance)
        {
            if (currentIndex == patrolPoints.Count - 1) agent.Blackboard.SetItem("ShouldPatrol", false);
            
            currentIndex = (currentIndex + 1) % patrolPoints.Count;
            currentPoint = patrolPoints[ currentIndex ];
            navMeshAgent.SetDestination(agent.Body.transform.position);
            return BehaviourNodeStatus.Success;
        }

        navMeshAgent.SetDestination(currentPoint);
        return BehaviourNodeStatus.None;
	}
}