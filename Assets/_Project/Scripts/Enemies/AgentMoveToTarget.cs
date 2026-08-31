using UnityEngine;
using UnityEngine.AI;

public class AgentMoveToTarget
{
    private readonly NavMeshAgent _agent;

    public AgentMoveToTarget(NavMeshAgent navMeshAgent)
    {
        _agent = navMeshAgent;
    }

    public void Move(Transform target)
    {
        if (target != null)
        {
            _agent.isStopped = false;
            _agent.updateRotation = true;
            _agent.destination = target.position;
        }
    }

    public void StopMove()
    {
        _agent.isStopped = true;
        _agent.updateRotation = false;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
    }
}