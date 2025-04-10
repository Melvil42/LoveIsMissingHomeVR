using UnityEngine;

public class IgnoreParentZRollLook : MonoBehaviour
{
    [Header("Sprite offset in 3D Space")]
    public Vector3 offsetEuler = new Vector3(90f, 0f, 0f);

    void LateUpdate()
    {
        //Get parent's forward direction in world space
        Vector3 fwd = transform.parent.forward;

        //setting up vector to world space - (0, 1, 0)
        Vector3 up = Vector3.up;

        //Create a rotation that looks in 'fwd' while keeping 'up' as close to Vector3.up as possible
        Quaternion noRoll = Quaternion.LookRotation(fwd, up);

        //Multiply by offset
        Quaternion finalRot = noRoll * Quaternion.Euler(offsetEuler);

        //Assign to child in world space to override all local rolling
        transform.rotation = finalRot;
    }
}