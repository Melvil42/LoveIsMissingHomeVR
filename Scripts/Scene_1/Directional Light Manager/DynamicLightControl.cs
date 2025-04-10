using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class DynamicLightControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform xrOrigin;
    [SerializeField] public Transform directionalLight;
    [SerializeField] public Renderer panopticon;
    [SerializeField] public float lightLockAnimDuration = 2f;
    [SerializeField] public AnimationCurve lightLockAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Events")]
    public UnityEvent completeLockLight;

    [Header("Light Values")]
    public float dark;
    public float bright = 50;

    private float _lightRange;
    private static float _distanceRange;
    private float _currDistance;
    private bool _isLock;
    private float _currentXRotation;
    private float _deliverXRotation;

    // Returns current light angle or locked value if light is locked
    public float EffectiveRotation {
        get {
            return _isLock ? dark : _deliverXRotation;
        }
    }

    // Returns normalized progress (0–1) based on current light rotation between dark and bright
    public float lightProgress
    {
        get
        {
            if (Math.Abs(_lightRange) < 0.01f) return 0f;
            return Mathf.InverseLerp(dark, bright, _deliverXRotation);
        }
    }

    // Triggers lighting lock animation
    public void OnLockLightingAnimation()
    {
        StartCoroutine(LockLightingRoutine());
    }

    private void Start()
    {
        if (directionalLight == null)
            Debug.LogError("light reference is Null");
        if (xrOrigin == null)
            Debug.LogError("xrOrigin reference is Null");
        if (panopticon == null)
            Debug.LogError("panopticon reference is Null");

        _lightRange = Math.Abs(dark - bright);
        _distanceRange = OnGetModelSize(panopticon);

        if (float.IsNaN(_distanceRange))
            Debug.LogError("No Renderer found on reference / reference is null");

        OnSetPlayerCurrentDistance();

        _currentXRotation = Math.Clamp(dark + _lightRange * (_currDistance / _distanceRange), dark, bright);
        _deliverXRotation = _currentXRotation;

        OnSetLightDirection();
    }

    private void Update()
    {
        if (_isLock) return;

        OnSetPlayerCurrentDistance();

        _currentXRotation = Math.Clamp(dark + _lightRange * (_currDistance / _distanceRange), dark, bright);
        _deliverXRotation = _currentXRotation;

        OnSetLightDirection();
    }

    // Calculates half the renderer’s width
    private float OnGetModelSize(Renderer meshRenderer)
    {
        float width = float.NaN;
        if (meshRenderer != null)
        {
            width = meshRenderer.bounds.size.x;
            return width / 2f;
        }
        Debug.LogError("No reference to OnGetModelSize");
        return width;
    }

    // Updates current player distance from origin in XZ plane
    private void OnSetPlayerCurrentDistance()
    {
        _currDistance = new Vector2(xrOrigin.position.x, xrOrigin.position.z).magnitude;
    }

    // Updates the directional light’s rotation based on current distance
    private void OnSetLightDirection()
    {
        directionalLight.localRotation = Quaternion.Euler(
            _currentXRotation,
            directionalLight.localRotation.eulerAngles.y,
            directionalLight.localRotation.eulerAngles.z
        );
    }

    // Locks the light state (disables updates)
    public void OnSetLock()
    {
        _isLock = true;
        Debug.Log("isLock set to true");
    }

    // Coroutine for animating light rotation toward x = 0
    private IEnumerator LockLightingRoutine()
    {
        OnSetLock();
        float elapsed = 0f;
        float startX = _deliverXRotation;

        while (elapsed < lightLockAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / lightLockAnimDuration);
            float curveVal = lightLockAnimCurve.Evaluate(t);

            float currentX = Mathf.LerpAngle(startX, 0f, curveVal);

            Vector3 eulers = directionalLight.localRotation.eulerAngles;
            eulers.x = currentX;
            directionalLight.localRotation = Quaternion.Euler(eulers);
            _deliverXRotation = currentX;

            yield return null;
        }

        Vector3 finalEulers = directionalLight.localRotation.eulerAngles;
        finalEulers.x = 0f;
        directionalLight.localRotation = Quaternion.Euler(finalEulers);
        _deliverXRotation = finalEulers.x;

        completeLockLight?.Invoke();
        Destroy(gameObject);
    }
}
