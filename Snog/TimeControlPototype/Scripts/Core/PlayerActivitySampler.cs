using UnityEngine;

public class PlayerActivitySampler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxMoveSpeed = 7.0f;

    [Header("Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxAngularSpeedDeg = 240f;

    private Vector3 _prevPos;
    private Quaternion _prevRot;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        _prevPos = transform.position;
        _prevRot = cameraTransform != null ? cameraTransform.rotation : transform.rotation;
    }

    private void Update()
    {
        float dt = Mathf.Max(Time.unscaledDeltaTime, 0.0001f);

        float moveIntensity = ComputeMoveIntensity(dt);
        float lookIntensity = ComputeLookIntensity(dt);

        if (TimeController.Instance != null)
        {
            TimeController.Instance.SetMove(moveIntensity);
            TimeController.Instance.SetLook(lookIntensity);
        }

        _prevPos = transform.position;
        _prevRot = cameraTransform != null ? cameraTransform.rotation : transform.rotation;
    }

    private float ComputeMoveIntensity(float dt)
    {
        Vector3 delta = transform.position - _prevPos;
        float speed = delta.magnitude / dt;
        return Mathf.Clamp01(speed / Mathf.Max(maxMoveSpeed, 0.0001f));
    }

    private float ComputeLookIntensity(float dt)
    {
        Quaternion currentRot = cameraTransform != null ? cameraTransform.rotation : transform.rotation;
        float angle = Quaternion.Angle(_prevRot, currentRot);
        float angularSpeed = angle / dt;
        return Mathf.Clamp01(angularSpeed / Mathf.Max(maxAngularSpeedDeg, 0.0001f));
    }
}
