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

    // Store the initial local x positions for calibration.
    private float leftInitialX;
    private float rightInitialX;
    private bool calibrated = false;

    void Start()
    {
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Please assign the left and right controller transforms in the Inspector.");
            return;
        }
        // Calibrate initial horizontal positions.
        leftInitialX = leftController.localPosition.x;
        rightInitialX = rightController.localPosition.x;
        calibrated = true;
    }

    void Update()
    {
        if (!calibrated) return;

        // Determine lateral offset (x-axis) relative to initial calibration.
        float leftOffsetX = leftController.localPosition.x - leftInitialX;
        float rightOffsetX = rightController.localPosition.x - rightInitialX;

        // Average lateral offset.
        float averageOffsetX = (leftOffsetX + rightOffsetX) / 2f;

        // Only rotate if the offset magnitude exceeds the threshold.
        if (Mathf.Abs(averageOffsetX) > turnActivationThreshold)
        {
            // Calculate turn amount. Positive offset for turning right, negative for left.
            float turnAmount = averageOffsetX * turnSensitivity * Time.deltaTime;
            transform.Rotate(0, turnAmount, 0);
        }
    }
}