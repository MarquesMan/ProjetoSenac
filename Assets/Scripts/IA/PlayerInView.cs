using UnityEngine;
using System;
using Brainiac;
using Assets.IA;

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

		if (Utils.GameObjectInView(
			player, agent.Body,
			agent.Blackboard.GetItem<float>("FieldOfView", 0f),
			agent.Blackboard.GetItem<float>("ViewDistance", 0f)
        ))
        {
			agent.Blackboard.SetItem("PlayerPos", player.gameObject.transform.position);
			return BehaviourNodeStatus.Success;
        }
		else
			return BehaviourNodeStatus.Failure;

	}
}