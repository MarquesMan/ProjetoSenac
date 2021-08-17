using UnityEngine;
using System;
using Brainiac;
using UnityEngine.AI;
using Brainiac.Serialization;

[AddNodeMenu("Action/ChasePlayerShadow")]
public class ChasePlayerShadow : Brainiac.Action
{
    private NavMeshAgent navMeshAgent;
    private float stoppingDistance;

    [BTProperty("Duration")]
    private MemoryVar m_duration;

    private float m_startTime;

    [BTIgnore]
    public MemoryVar Duration
    {
        get
        {
            return m_duration;
        }
    }

    public ChasePlayerShadow()
    {
        m_duration = new MemoryVar();
    }

    protected override void OnEnter(AIAgent agent)
    {
        m_startTime = Time.time;
    }

    public override void OnStart(AIAgent agent)
    {
        navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);
        stoppingDistance = navMeshAgent.stoppingDistance;
    }

    protected override BehaviourNodeStatus OnExecute(AIAgent agent)
    {

        // navMeshAgent = agent.Blackboard.GetItem<NavMeshAgent>("NavMeshAgent", null);

        if (navMeshAgent is null) return BehaviourNodeStatus.Failure;

        Vector3? playerShadown = agent.Blackboard.GetItem<Vector3?>("PlayerPos", null);
        

        if (!playerShadown.HasValue) return BehaviourNodeStatus.Failure;

        var agentPos = agent.Body.transform.position;
        agentPos.y -= 1f; // Diminui altura do player

        navMeshAgent.SetDestination((Vector3)playerShadown);

        var distanceRay = navMeshAgent.pathEndPosition - agentPos;

        float duration = m_duration.AsFloat.HasValue ? m_duration.AsFloat.Value : m_duration.Evaluate<float>(agent.Blackboard, 0.0f);

        if ((distanceRay.magnitude <= stoppingDistance) || (Time.time >= m_startTime + duration))
        {
            agent.Blackboard.SetItem("PlayerPos", null);
            navMeshAgent.SetDestination(agentPos);
            return BehaviourNodeStatus.Success;
        };

        return BehaviourNodeStatus.None;
    }
}