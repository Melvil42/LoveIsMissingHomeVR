using UnityEngine;

public class RecenterXR : MonoBehaviour
{
    [Tooltip("Assign the VR Camera (the headset) that is a child of this player rig.")]
    public Transform vrCamera;

    void Start()
    {
        if (vrCamera == null)
        {
            Debug.LogError("VR Camera transform is not assigned!");
            return;
        }

        // Step 1: Move the rig so the vrCamera's world position becomes the rig's origin.
        // This resets any positional offset between the rig and the camera.
        Vector3 cameraWorldPos = vrCamera.position;
        transform.position = cameraWorldPos;
        Debug.Log("Player rig moved to: " + cameraWorldPos);

        // Step 2: Adjust rotation so the camera faces forward (world z+).
        // Compute the camera's horizontal forward vector.
        Vector3 camForward = vrCamera.forward;
        camForward.y = 0;  // Ignore any vertical tilt.
        if (camForward.sqrMagnitude < 0.001f)
        {
            Debug.LogWarning("Camera forward vector is too small to determine rotation.");
            return;
        }
        camForward.Normalize();

        // Determine the angle between the camera's forward and the world forward.
        float angleDiff = Vector3.SignedAngle(camForward, Vector3.forward, Vector3.up);
        Debug.Log("Angle difference: " + angleDiff);

        // Rotate the rig around the camera's position by this angle.
        transform.RotateAround(cameraWorldPos, Vector3.up, angleDiff);
        Debug.Log("Player rig rotated to align camera with world forward.");
    }
}