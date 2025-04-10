using UnityEngine;
using System.Collections;

public class PlayerChairLockAnimation : MonoBehaviour
{
    [Header("References")]
    public Transform xrRigTransform;
    public float rotationMovementDuration = 1.5f;
    public float headMovementDuration = 1.5f;
    public float finalHeadPosition;
    public Transform cameraOffset;
    public Transform chairPosition;

    private bool _isMoving = false;

    // Starts the movement coroutine if not already running
    public void OnMovePlayerToChair()
    {
        if (!_isMoving)
        {
            Debug.Log("Starting Coroutine MovePlayerToChair");
            StartCoroutine(MoveAndRotateRoutine(chairPosition)); 
        }
    }

    // Moves and rotates the XR rig to the chair's position
    private IEnumerator MoveAndRotateRoutine(Transform chairTransform)
    {
        _isMoving = true;

        Vector3 startPos = xrRigTransform.position;
        Quaternion startRot = xrRigTransform.rotation;

        Vector3 endPos = chairTransform.position;
        Quaternion endRot = Quaternion.LookRotation(Vector3.right, Vector3.up);

        float elapsed = 0f;

        while (elapsed < rotationMovementDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationMovementDuration;

            xrRigTransform.position = Vector3.Lerp(startPos, endPos, t);
            xrRigTransform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        xrRigTransform.position = endPos;
        xrRigTransform.rotation = endRot;

        _isMoving = false;
        Debug.Log("Player movement animation done");
        StartCoroutine(MoveHeadsetDownRoutine(finalHeadPosition));
    }

    // Animates the vertical adjustment of the camera offset
    private IEnumerator MoveHeadsetDownRoutine(float finalPosition)
    {
        Vector3 startPos = cameraOffset.position;
        Vector3 finalPos = new Vector3(startPos.x, finalPosition, startPos.z);

        float elapsed = 0f;

        while (elapsed < headMovementDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / headMovementDuration;

            cameraOffset.position = Vector3.Lerp(startPos, finalPos, t);
            yield return null;
        }
    }
}
