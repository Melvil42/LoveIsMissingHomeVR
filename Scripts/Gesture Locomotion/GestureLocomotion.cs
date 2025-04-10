using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GestureLocomotion : MonoBehaviour
{
    [Header("Controller References")]
    public Transform leftController;
    public Transform rightController;

    [Header("Locomotion Settings")]
    [Tooltip("Multiplier for mapping controller extension to speed.")]
    public float movementSensitivity = 1.0f;

    [Tooltip("Distance (in local space) beyond which movement begins.")]
    public float calibrationDistance = 0.2f;

    [Tooltip("Maximum movement speed.")]
    public float maxSpeed = 5f;

    // Store the initial local positions for calibration
    private Vector3 leftInitialLocalPos;
    private Vector3 rightInitialLocalPos;
    private bool calibrated = false;

    void Start()
    {
        // Validate controller references
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Please assign the left and right controller transforms in the Inspector.");
            return;
        }

        // Store initial local positions for calibration
        leftInitialLocalPos = leftController.localPosition;
        rightInitialLocalPos = rightController.localPosition;
        calibrated = true;
    }

    void Update()
    {
        if (!calibrated)
            return;

        // Compute local offset from initial calibration positions
        Vector3 leftOffset = leftController.localPosition - leftInitialLocalPos;
        Vector3 rightOffset = rightController.localPosition - rightInitialLocalPos;

        // Compute extension past the calibration threshold along the Z-axis
        float leftExtension = Mathf.Max(0, leftOffset.z - calibrationDistance);
        float rightExtension = Mathf.Max(0, rightOffset.z - calibrationDistance);

        // Average the two extensions
        float averageExtension = (leftExtension + rightExtension) / 2f;

        // Calculate movement speed
        float currentSpeed = Mathf.Clamp(averageExtension * movementSensitivity, 0, maxSpeed);

        // Average forward direction of both controllers
        Vector3 leftForward = leftController.forward;
        Vector3 rightForward = rightController.forward;
        Vector3 movementDirection = (leftForward + rightForward) * 0.5f;
        movementDirection.y = 0;

        if (movementDirection.sqrMagnitude > 0.001f)
        {
            movementDirection.Normalize();
        }

        // Calculate displacement
        Vector3 displacement = movementDirection * (currentSpeed * Time.deltaTime);

        // Apply world-space movement
        transform.Translate(displacement, Space.World);
    }
}
