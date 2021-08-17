using UnityEngine;
using System;
using Brainiac;
using Brainiac.Serialization;
using UnityEngine.AI;

[AddNodeMenu("Action/CheckForDoor")]
public class CheckForDoor : Brainiac.Action
{

	[BTProperty("Check Distance")]
	float checkDistance = 0.5f;
	private LayerMask layerMask;
    private NavMeshAgent navMeshAgent;
	private Vector3 halfExtents = new Vector3(1,1,1);

    public override void OnStart(AIAgent agent)
    {
		layerMask = LayerMask.GetMask("Door"); //, "House");
		navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
	}

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{
		RaycastHit hit;
		// Checar frente do Agent para portas
		if(Physics.BoxCast(agent.Body.transform.position, halfExtents,
			navMeshAgent.desiredVelocity, out hit, Quaternion.identity, 
			checkDistance, layerMask))
        {
			var door = hit.collider.GetComponentInParent<Door>();

			if (door == null || !door.closed || !DadBehaviour.canDadOpenThisDoor(door)) return BehaviourNodeStatus.Failure;
			agent.Blackboard.SetItem("Door", door);
			return BehaviourNodeStatus.Success;
        }
		
		return BehaviourNodeStatus.Failure;
	}
}