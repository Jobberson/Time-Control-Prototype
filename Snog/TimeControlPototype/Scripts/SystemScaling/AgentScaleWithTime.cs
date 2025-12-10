using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentScaleWithTime : MonoBehaviour
{
    public float baseSpeed = 3.5f;
    public float baseAngularSpeed = 120f;
    public float baseAcceleration = 8f;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        baseSpeed = _agent.speed;
        baseAngularSpeed = _agent.angularSpeed;
        baseAcceleration = _agent.acceleration;
    }

    private void Update()
    {
        if (TimeController.Instance == null)
            return;

        float s = TimeController.Instance.CurrentScale;
        _agent.speed = baseSpeed * s;
        _agent.angularSpeed = baseAngularSpeed * s;
        _agent.acceleration = baseAcceleration * s;
    }
}
