using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private PlayerInputReader input;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputReader>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = input.Move;

        Vector3 forward = Vector3.ProjectOnPlane(
            cameraTransform.forward,
            Vector3.up
        ).normalized;

        Vector3 right = Vector3.ProjectOnPlane(
            cameraTransform.right,
            Vector3.up
        ).normalized;

        Vector3 direction = forward * moveInput.y + right * moveInput.x;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement =
            direction * moveSpeed +
            Vector3.up * verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }
}