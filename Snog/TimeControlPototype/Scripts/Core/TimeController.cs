using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    [Header("SUPERHOT Time")]
    [SerializeField, Range(0f, 1f)] private float stoppedScale = 0.02f;
    [SerializeField, Range(0f, 1f)] private float activeScale = 1f;

    [Tooltip("How fast time snaps between stopped and active (unscaled).")]
    [SerializeField] private float snapSpeed = 40f;

    [Tooltip("Minimum activity needed to wake time.")]
    [SerializeField] private float activityThreshold = 0.01f;

    [Header("Physics")]
    [SerializeField] private float baseFixedDeltaTime = 0.02f;
    [SerializeField] private float minFixedDeltaTime = 0.002f;

    public float CurrentScale { get; private set; }

    private float _move;
    private float _look;
    private float _impulse;
    private float _desiredScale;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentScale = activeScale;
        _desiredScale = activeScale;
        ApplyTime();
    }

    private void Update()
    {
        float activity = Mathf.Max(_move, _look, _impulse);
        float target = activity > activityThreshold ? activeScale : stoppedScale;

        _desiredScale = Mathf.MoveTowards(
            _desiredScale,
            target,
            snapSpeed * Time.unscaledDeltaTime
        );

        CurrentScale = _desiredScale;
        ApplyTime();

        _impulse = 0f; // impulses are one-frame spikes
    }

    private void ApplyTime()
    {
        Time.timeScale = CurrentScale;
        Time.fixedDeltaTime = Mathf.Max(baseFixedDeltaTime * CurrentScale, minFixedDeltaTime);
    }

    public void SetMove(float v) => _move = Mathf.Clamp01(v);
    public void SetLook(float v) => _look = Mathf.Clamp01(v);
    public void AddImpulse(float v) => _impulse = Mathf.Clamp01(_impulse + v);
}
