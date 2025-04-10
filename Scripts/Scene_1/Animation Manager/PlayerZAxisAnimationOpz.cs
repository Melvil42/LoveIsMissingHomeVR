using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerZAxisAnimationOpz : MonoBehaviour
{
    [Header("Object to Move")]
    public GameObject xrOriginGameObject;

    [Header("Movement Settings")]
    public float targetZ = 10f;
    public float totalDuration = 2f;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // How close we need to be before we say we've "approached" the target.
    [SerializeField] private float distanceThreshold = 0.1f;

    public UnityEvent animationFinished;
    
    //instance variables
    private float _elapsedTime = 0f;
    private float _initialZ = 0f;
    private bool _approachEventFired = false;

    public void StartMovement()
    {
        if (xrOriginGameObject == null)
        {
            Debug.LogWarning("PlayerZAxisAnimation: xrOriginGameObject is not assigned.");
            return;
        }

        _initialZ = xrOriginGameObject.transform.position.z;
        _approachEventFired = false;
        StartCoroutine(MovePlayerCoroutine());
    }

    private IEnumerator MovePlayerCoroutine()
    {
        _elapsedTime = 0f;
        while (_elapsedTime < totalDuration)
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / totalDuration);
            float curveValue = movementCurve.Evaluate(t);
            float currentZ = Mathf.Lerp(_initialZ, targetZ, curveValue);

            Vector3 newPosition = xrOriginGameObject.transform.position;
            newPosition.z = currentZ;
            xrOriginGameObject.transform.position = newPosition;

            if (!_approachEventFired && Mathf.Abs(targetZ - currentZ) < distanceThreshold)
            {
                _approachEventFired = true; 
                // Or an "approached" event if you want one.
            }

            yield return null; // Wait for next frame.
        }
        // Ensure we snap to target exactly.
        Vector3 finalPos = xrOriginGameObject.transform.position;
        finalPos.z = targetZ;
        xrOriginGameObject.transform.position = finalPos;
        animationFinished?.Invoke();
        Destroy(gameObject);
    }
    
}