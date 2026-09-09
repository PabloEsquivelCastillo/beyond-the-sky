using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private PlayerInputReader input;

    [Header("Rotación")]
    [SerializeField] private float sensitivity = 0.12f;
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 65f;

    [Header("Distancia")]
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private float distance = 5f;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private LayerMask collisionMask;

    private float yaw;
    private float pitch = 15f;

    private void Start()
    {
        yaw = target.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        RotateCamera();
        FollowTarget();
    }

    private void RotateCamera()
    {
        yaw += input.Look.x * sensitivity;
        pitch -= input.Look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = target.position + targetOffset;
        Vector3 directionToCamera = -(rotation * Vector3.forward);

        float finalDistance = distance;

        if (Physics.SphereCast(
            pivot,
            collisionRadius,
            directionToCamera,
            out RaycastHit hit,
            distance,
            collisionMask,
            QueryTriggerInteraction.Ignore))
        {
            finalDistance = Mathf.Max(hit.distance - collisionRadius, 0.3f);
        }

        transform.position = pivot + directionToCamera * finalDistance;
        transform.rotation = rotation;
    }
}