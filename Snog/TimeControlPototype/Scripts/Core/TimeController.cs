using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    [Header("Time Scale")]
    [SerializeField, Range(0.0f, 1.0f)] private float minScale = 0.02f;
    [SerializeField, Range(0.5f, 1.0f)] private float maxScale = 1.0f;
    [SerializeField] private float smoothingTime = 0.08f;

    [Header("Physics")]
    [SerializeField] private float baseFixedDeltaTime = 0.02f;
    [SerializeField] private float minFixedDeltaTime = 0.002f;

    [Header("Weights")]
    [SerializeField, Range(0.0f, 1.0f)] private float moveWeight = 0.7f;
    [SerializeField, Range(0.0f, 1.0f)] private float lookWeight = 0.3f;

    [Header("Debug")]
    public bool showOnScreen = true;

    public float CurrentScale { get; private set; } = 1.0f;

    // external inputs
    private float _moveIntensity;
    private float _lookIntensity;

    // impulses
    private float _impulseIntensity;
    private float _desiredScale;
    private float _smoothVel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentScale = maxScale;
        _desiredScale = maxScale;
        Time.timeScale = CurrentScale;
        Time.fixedDeltaTime = Mathf.Max(baseFixedDeltaTime * CurrentScale, minFixedDeltaTime);
    }

    private void Update()
    {
        float activity = Mathf.Clamp01(_moveIntensity * moveWeight + _lookIntensity * lookWeight + _impulseIntensity);
        float target = Mathf.Lerp(minScale, maxScale, activity);

        _desiredScale = Mathf.SmoothDamp(_desiredScale, target, ref _smoothVel, smoothingTime);

        CurrentScale = _desiredScale;
        Time.timeScale = CurrentScale;

        Time.fixedDeltaTime = Mathf.Max(baseFixedDeltaTime * CurrentScale, minFixedDeltaTime);

        DecayImpulse();

        if (showOnScreen)
        {
            DrawOverlay();
        }
    }

    public void SetMoveIntensity(float normalized)
    {
        _moveIntensity = Mathf.Clamp01(normalized);
    }

    public void SetLookIntensity(float normalized)
    {
        _lookIntensity = Mathf.Clamp01(normalized);
    }

    /// <summary>
    /// Adds a temporary boost to activity, ex: on firing.
    /// </summary>
    public void AddImpulse(float amount, float duration)
    {
        StopCoroutine(nameof(ImpulseRoutine));
        StartCoroutine(ImpulseRoutine(amount, duration));
    }

    private IEnumerator ImpulseRoutine(float amount, float duration)
    {
        _impulseIntensity = Mathf.Clamp01(_impulseIntensity + amount);
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        _impulseIntensity = 0f;
    }

    private void DecayImpulse()
    {
        // Keep blank or add smoothing here.
    }

    private void DrawOverlay()
    {
        // simple top-left text
        GUI.color = Color.white;
        GUI.Label(new Rect(10, 10, 280, 24), $"Time Scale: {CurrentScale:F3}");
    }
}
