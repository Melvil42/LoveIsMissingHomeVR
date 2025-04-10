using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GestureLocomotion : MonoBehaviour
{
    [Header("Controller References")]
    // Assign these via the Inspector to the left and right controller transforms.
    public Transform leftController;
    public Transform rightController;

    [Header("Locomotion Settings")]
    [Tooltip("Multiplier for mapping controller extension to speed.")]
    public float movementSensitivity = 1.0f;
    [Tooltip("Distance (in local space) beyond which movement begins.")]
    public float calibrationDistance = 0.2f;
    [Tooltip("Maximum movement speed.")]
    public float maxSpeed = 5f;

    // Store the initial local positions for calibration.
    private Vector3 leftInitialLocalPos;
    private Vector3 rightInitialLocalPos;
    private bool calibrated = false;

    void Start()
    {
        // Ensure both controller references are set.
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Please assign the left and right controller transforms in the Inspector.");
            return;
        }

        // Calibrate initial positions (assumes controllers are at their resting positions).
        leftInitialLocalPos = leftController.localPosition;
        rightInitialLocalPos = rightController.localPosition;
        calibrated = true;
    }

    void Update()
    {
        if (!calibrated)
            return;

        // Calculate local offsets relative to the calibrated (initial) positions.
        Vector3 leftOffset = leftController.localPosition - leftInitialLocalPos;
        Vector3 rightOffset = rightController.localPosition - rightInitialLocalPos;

        // We assume forward movement is along the local Z-axis.
        float leftExtension = Mathf.Max(0, leftOffset.z - calibrationDistance);
        float rightExtension = Mathf.Max(0, rightOffset.z - calibrationDistance);

        // Average the extensions from both controllers.
        float averageExtension = (leftExtension + rightExtension) / 2f;

        // Calculate the current speed (scaled and clamped).
        float currentSpeed = Mathf.Clamp(averageExtension * movementSensitivity, 0, maxSpeed);

        // Get the forward direction from both controllers.
        Vector3 leftForward = leftController.forward;
        Vector3 rightForward = rightController.forward;

        // Compute the average forward direction.
        Vector3 movementDirection = (leftForward + rightForward) * 0.5f;
        movementDirection.y = 0;  // Lock movement to the horizontal plane.
        if (movementDirection.sqrMagnitude > 0.001f)
        {
            movementDirection.Normalize();
        }

        // Calculate the displacement based on speed and frame time.
        Vector3 displacement = movementDirection * (currentSpeed * Time.deltaTime);

        // Move the XR rig (this GameObject) in world space.
        transform.Translate(displacement, Space.World);
    }
}
