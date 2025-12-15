using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float sprintSpeed = 9f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpForce = 1.6f;

    [Header("Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 120f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("SUPERHOT Tuning")]
    [SerializeField] private float lookWakeSensitivity = 0.015f;

    private CharacterController controller;
    private Vector3 verticalVelocity;
    private float pitch;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        FeedTimeController();
    }

    private void HandleLook()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        pitch -= my * mouseSensitivity * Time.unscaledDeltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.Rotate(Vector3.up * mx * mouseSensitivity * Time.unscaledDeltaTime);
    }

    private void HandleMovement()
    {
        bool grounded = controller.isGrounded;
        if (grounded && verticalVelocity.y < 0)
            verticalVelocity.y = -2f;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift);
        float speed = sprint ? sprintSpeed : walkSpeed;

        Vector3 input = new Vector3(x, 0f, z);
        input = Vector3.ClampMagnitude(input, 1f);

        // INSTANT horizontal movement (unscaled)
        Vector3 horizontal = transform.TransformDirection(input) * speed;
        controller.Move(horizontal * Time.unscaledDeltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            TimeController.Instance?.AddImpulse(1f);
        }

        // Vertical motion stays time-scaled for drama
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity.y * Time.deltaTime);
    }

    private void FeedTimeController()
    {
        if (TimeController.Instance == null) return;

        float move =
            new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).magnitude;

        float look =
            (Mathf.Abs(Input.GetAxisRaw("Mouse X")) +
             Mathf.Abs(Input.GetAxisRaw("Mouse Y"))) * lookWakeSensitivity;

        TimeController.Instance.SetMove(move);
        TimeController.Instance.SetLook(look);
    }
}
