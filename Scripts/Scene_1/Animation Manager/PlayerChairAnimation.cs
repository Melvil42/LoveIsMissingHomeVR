using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class PlayerChairAnimation : MonoBehaviour
{
    [Header("References")]
    public Transform xrRigTransform; // The GameObject controlling your camera & movement
    public float rotationMovementDuration = 1.5f;
    public float headMovementDuration = 1.5f;  // Time it takes to move
    public float finalHeadPosition;
    public Transform cameraOffset;
    public Transform chairPosition;
    
    private bool _isMoving = false;

    public void OnMovePlayerToChair()
    {
        if (!_isMoving)
        {
            Debug.Log("Starting Coroutine MovePlayerToChair");
            StartCoroutine(MoveAndRotateRoutine(chairPosition)); 
        }
    }

    private IEnumerator MoveAndRotateRoutine(Transform chairTransform)
    {
        _isMoving = true;

        // (Optional) Disable movement scripts
        // e.g., locomotionSystem.enabled = false;

        Vector3 startPos = xrRigTransform.position;
        Quaternion startRot = xrRigTransform.rotation;

        // Final target alignment:
        // 1) Move to chair’s position
        // 2) Align forward axis with world X-axis OR chair’s forward
        Vector3 endPos = chairTransform.position;
        
        // If you specifically want to align to World X-axis:
        Quaternion endRot = Quaternion.LookRotation(Vector3.right, Vector3.up);

        // If you want to align to chair’s local forward instead:
        // Quaternion endRot = chairTransform.rotation;

        float elapsed = 0f;

        while (elapsed < rotationMovementDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationMovementDuration;

            // Lerp/Slerp
            xrRigTransform.position = Vector3.Lerp(startPos, endPos, t);
            xrRigTransform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        // Make sure final position/rotation are exact
        xrRigTransform.position = endPos;
        xrRigTransform.rotation = endRot;

        // (Optional) Re-enable movement scripts
        // locomotionSystem.enabled = true;

        _isMoving = false;
        Debug.Log("Player movement animation done");
        StartCoroutine(MoveHeadsetDownRoutine(finalHeadPosition));
    }
    
    private IEnumerator MoveHeadsetDownRoutine(float finalPosition)
    {
        Vector3 startPos = cameraOffset.position;

        Vector3 finalPos = new Vector3(startPos.x, finalPosition, startPos.z);

        float elapsed = 0f;

        while (elapsed < headMovementDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / headMovementDuration;

            // Lerp/Slerp
            cameraOffset.position = Vector3.Lerp(startPos, finalPos, t);
            yield return null;
        }
    }
}