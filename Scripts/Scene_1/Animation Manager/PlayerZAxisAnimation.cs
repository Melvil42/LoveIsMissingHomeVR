using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerZAxisAnimation : MonoBehaviour
{
    [Header("Object to Move")]
    public GameObject xrOriginGameObject;

    [Header("Movement Settings")]
    public float targetZ = 10f;
    public float totalDuration = 2f;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [SerializeField] private float distanceThreshold = 0.1f;

    public UnityEvent animationFinished;

    private float _elapsedTime = 0f;
    private float _initialZ = 0f;
    private bool _approachEventFired = false;

    // Starts the movement animation
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

    // Moves the player along the Z-axis using a smooth curve
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
            }

            yield return null;
        }

        Vector3 finalPos = xrOriginGameObject.transform.position;
        finalPos.z = targetZ;
        xrOriginGameObject.transform.position = finalPos;

        animationFinished?.Invoke();
        Destroy(gameObject);
    }
}
