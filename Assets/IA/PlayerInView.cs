using UnityEngine;
using System;
using Brainiac;

[AddNodeMenu("Action/PlayerInView")]
public class PlayerInView : Brainiac.Action
{
	private GameObject player = null;

    public override void OnStart(AIAgent agent)
    {
        player = agent.Blackboard.GetItem<GameObject>("Player", null);
	}

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
	{

		if (player is null) return BehaviourNodeStatus.Failure;
		
		RaycastHit hit;

		var distanceRay = player.transform.position - agent.Body.transform.position;

		// Ignorar coisas que o player pode carregar
		LayerMask layerMask = ~LayerMask.GetMask("Grabbable", "Key");

		if (!Physics.Raycast(agent.Body.transform.position, // Origin
				distanceRay.normalized, // Direction
				out hit, // Escrever os valores na variavel hit
				15f, // Distancia
				layerMask) // Aplicar a mascara descrita acima
			) return BehaviourNodeStatus.Failure;  // Nao ta vendo nada
		
		if (!hit.collider.CompareTag("Player")) return BehaviourNodeStatus.Failure;

		if (Vector3.Angle(
			agent.Body.transform.forward,
			distanceRay ) > agent.Blackboard.GetItem<float>("FieldOfView", 0f)) 
			return BehaviourNodeStatus.Failure;

		if (distanceRay.magnitude > agent.Blackboard.GetItem<float>("ViewDistance", 0f))
			return BehaviourNodeStatus.Failure;

		return BehaviourNodeStatus.Success;
	}
}