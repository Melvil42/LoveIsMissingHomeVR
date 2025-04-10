using UnityEngine;

public class IgnoreParentZAxisRoll : MonoBehaviour
{
    [Header("Sprite offset in 3D Space")]
    public Vector3 offsetEuler = new Vector3(90f, 0f, 0f);

    void LateUpdate()
    {
        // Get the parent's forward direction in world space
        Vector3 fwd = transform.parent.forward;

        // Use the global up direction as reference
        Vector3 up = Vector3.up;

        // Create a rotation that looks in the parent's forward direction, keeping up as world up
        Quaternion noRoll = Quaternion.LookRotation(fwd, up);

        // Apply an additional rotation offset
        Quaternion finalRot = noRoll * Quaternion.Euler(offsetEuler);

        // Set the rotation in world space to ignore parent's Z-axis roll
        transform.rotation = finalRot;
    }
}