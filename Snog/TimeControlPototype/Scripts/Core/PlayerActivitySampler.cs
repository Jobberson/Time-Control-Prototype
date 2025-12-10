using UnityEngine;

public class PlayerActivitySampler : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController characterController;
    public Rigidbody rigidbodyComponent;
    public float maxMoveSpeed = 7.0f;

    [Header("Look")]
    public Transform cameraTransform;
    public float maxAngularSpeedDeg = 240f;

    private Vector3 _prevPos;
    private Quaternion _prevRot;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        _prevPos = transform.position;
        _prevRot = cameraTransform != null ? cameraTransform.rotation : transform.rotation;
    }

    private void Update()
    {
        float moveIntensity = ComputeMoveIntensity();
        float lookIntensity = ComputeLookIntensity();

        if (TimeController.Instance != null)
        {
            TimeController.Instance.SetMoveIntensity(moveIntensity);
            TimeController.Instance.SetLookIntensity(lookIntensity);
        }

        _prevPos = transform.position;
        _prevRot = cameraTransform != null ? cameraTransform.rotation : transform.rotation;
    }

    private float ComputeMoveIntensity()
    {
        float speed = 0f;

        if (characterController != null)
        {
            Vector3 delta = transform.position - _prevPos;
            speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        }
        else if (rigidbodyComponent != null)
        {
            speed = rigidbodyComponent.velocity.magnitude;
        }
        else
        {
            Vector3 delta = transform.position - _prevPos;
            speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        }

        return Mathf.Clamp01(speed / Mathf.Max(maxMoveSpeed, 0.0001f));
    }

    private float ComputeLookIntensity()
    {
        Quaternion currentRot = cameraTransform != null ? cameraTransform.rotation : transform.rotation;

        float angle = Quaternion.Angle(_prevRot, currentRot);
        float angularSpeed = angle / Mathf.Max(Time.deltaTime, 0.0001f); // deg/sec

        return Mathf.Clamp01(angularSpeed / Mathf.Max(maxAngularSpeedDeg, 0.0001f));
    }
}
