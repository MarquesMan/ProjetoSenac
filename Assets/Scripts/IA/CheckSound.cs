using UnityEngine;
using System;
using Brainiac;
using System.Collections.Generic;
using Brainiac.Serialization;
using UnityEngine.AI;
using Assets.IA;

[AddNodeMenu("Action/CheckSound")]
public class CheckSound : Brainiac.Action
{
    private Stack<Vector3> listOfSounds = null;
    private NavMeshAgent navMeshAgent;

    private float minDistance = 2f;

    public override void OnStart(AIAgent agent)
    {
        listOfSounds = agent.Blackboard.GetItem<Stack<Vector3>>("ListOfSounds", null);
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        minDistance = navMeshAgent.stoppingDistance;
    }
    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
        if (listOfSounds is null || listOfSounds.Count == 0) return BehaviourNodeStatus.Failure;

        if (Utils.GameObjectInView(
            agent.Blackboard.GetItem<GameObject>("Player", null), agent.Body,
            agent.Blackboard.GetItem<float>("FieldOfView", 0f),
            agent.Blackboard.GetItem<float>("ViewDistance", 0f)
        ))
            return BehaviourNodeStatus.Success;

        var currentSound = listOfSounds.Peek();

        if (Vector3.Distance(currentSound, agent.Body.transform.position) <= minDistance)
        {
            listOfSounds.Pop();
            return BehaviourNodeStatus.Success;
        }

        navMeshAgent.SetDestination(currentSound);
        return BehaviourNodeStatus.Running;
        
    }
}