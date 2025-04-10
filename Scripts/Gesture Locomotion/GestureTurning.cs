using UnityEngine;

public class GestureTurning : MonoBehaviour
{
    [Header("Controller References")]
    public Transform leftController;
    public Transform rightController;

    [Header("Turning Settings")]
    [Tooltip("Multiplier for turning sensitivity.")]
    public float turnSensitivity = 100f;

    [Tooltip("Minimum lateral offset (in local x) to start turning.")]
    public float turnActivationThreshold = 0.1f;

    // Store the initial local X positions for calibration
    private float leftInitialX;
    private float rightInitialX;
    private bool calibrated = false;

    void Start()
    {
        // Validate controller references
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Please assign the left and right controller transforms in the Inspector.");
            return;
        }

        // Store initial local X positions for calibration
        leftInitialX = leftController.localPosition.x;
        rightInitialX = rightController.localPosition.x;
        calibrated = true;
    }

    void Update()
    {
        if (!calibrated)
            return;

        // Compute X-axis offsets from initial calibration
        float leftOffsetX = leftController.localPosition.x - leftInitialX;
        float rightOffsetX = rightController.localPosition.x - rightInitialX;

        // Average the lateral offsets
        float averageOffsetX = (leftOffsetX + rightOffsetX) / 2f;

        // Apply rotation if offset exceeds threshold
        if (Mathf.Abs(averageOffsetX) > turnActivationThreshold)
        {
            float turnAmount = averageOffsetX * turnSensitivity * Time.deltaTime;
            transform.Rotate(0, turnAmount, 0);
        }
    }
}