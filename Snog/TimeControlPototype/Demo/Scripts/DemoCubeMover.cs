using UnityEngine;

public class DemoCubeMover : MonoBehaviour
{
    public enum MoveMode
    {
        None,
        Oscillate,
        PingPong,
        Orbit,
        Rotate,
        Chaos
    }

    [Header("Time")]
    [SerializeField] private bool ignoreTimeScale = true;

    [Header("Mode")]
    public MoveMode mode = MoveMode.Oscillate;

    [Header("Movement")]
    public Vector3 direction = Vector3.right;
    public float distance = 3f;
    public float speed = 2f;

    [Header("Orbit")]
    public Vector3 orbitCenter;
    public float orbitRadius = 3f;
    public float orbitSpeed = 90f; // degrees per second

    [Header("Rotation")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 180f;

    [Header("Chaos")]
    public float chaosInterval = 0.3f;

    private Vector3 _startPos;
    private float _timer;
    private Vector3 _chaosTarget;

    private void Start()
    {
        _startPos = transform.position;
        orbitCenter = orbitCenter == Vector3.zero ? _startPos : orbitCenter;
        PickChaosTarget();
    }

    private void Update()
    {
        float dt = ignoreTimeScale
            ? Time.unscaledDeltaTime
            : Time.deltaTime;


        switch (mode)
        {
            case MoveMode.Oscillate:
                Oscillate(dt);
                break;

            case MoveMode.PingPong:
                PingPong(dt);
                break;

            case MoveMode.Orbit:
                Orbit(dt);
                break;

            case MoveMode.Rotate:
                Rotate(dt);
                break;

            case MoveMode.Chaos:
                Chaos(dt);
                break;
        }
    }

    private void Oscillate(float dt)
    {
        _timer += dt * speed;
        float t = Mathf.Sin(_timer);
        transform.position = _startPos + direction.normalized * t * distance;
    }

    private void PingPong(float dt)
    {
        _timer += dt * speed;
        float t = Mathf.PingPong(_timer, 1f);
        transform.position = Vector3.Lerp(
            _startPos,
            _startPos + direction.normalized * distance,
            t
        );
    }

    private void Orbit(float dt)
    {
        _timer += dt * orbitSpeed;
        float angleRad = _timer * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(angleRad),
            0f,
            Mathf.Sin(angleRad)
        ) * orbitRadius;

        transform.position = orbitCenter + offset;
    }

    private void Rotate(float dt)
    {
        transform.Rotate(rotationAxis.normalized, rotationSpeed * dt, Space.World);
    }

    private void Chaos(float dt)
    {
        _timer += dt;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _chaosTarget,
            speed * dt
        );

        if (_timer >= chaosInterval)
        {
            _timer = 0f;
            PickChaosTarget();
        }
    }

    private void PickChaosTarget()
    {
        _chaosTarget = _startPos + Random.insideUnitSphere * distance;
    }
}
