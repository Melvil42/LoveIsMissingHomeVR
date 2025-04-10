using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LightControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform xrOrigin;
    [SerializeField] public Transform directionalLight;
    [SerializeField] public Renderer panopticon;
    [SerializeField] public float lightLockAnimDuration = 2f;
    [SerializeField] public AnimationCurve lightLockAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Events")] public UnityEvent completeLockLight;
    
    
    [Header("Light Values")]
    public float dark;
    public float bright = 50;
    

    // Internal variables
    private float _lightRange;
    private static float _distanceRange;
    private float _currDistance;
    private bool _isLock;
    private float _currentXRotation;
    private float _deliverXRotation;
    
    public float EffectiveRotation {
        get {
            // If the light is locked, return the dark value, otherwise return _deliverXRotation.
            return _isLock ? dark : _deliverXRotation;
        }
    }
    
    public float lightProgress
    {
        get
        {
            // If dark >= bright, just return 0 to be safe
            if (Math.Abs(_lightRange) < 0.01f) return 0f;
            
            // (currentRotation - dark) / (bright - dark)
            return Mathf.InverseLerp(dark, bright, _deliverXRotation);
        }
    }
    
    /// <summary>
    /// Called by Scene_1 manager when the player enters the sphere trigger.
    /// Animates from current x-rotation to x=0 using lightLockAnimCurve / lightLockAnimDuration.
    /// </summary>
    public void OnLockLightingAnimation()
    {
        // Start a coroutine that animates x-rotation to 0
        StartCoroutine(LockLightingRoutine());
    }
    
    private void Start()
    {
        if (directionalLight == null)
        {
            Debug.LogError("light reference is Null");
        }
        if (xrOrigin == null)
        {
            Debug.LogError("xrOrigin reference is Null");
        }
        if (panopticon == null)
        {
            Debug.LogError("panopticon reference is Null");
        }

        // Calculate how far the light rotates across the range (dark..bright)
        _lightRange = Math.Abs(dark - bright);

        // Initialize _distanceRange
        _distanceRange = OnGetModelSize(panopticon);
        if (float.IsNaN(_distanceRange))
        {
            Debug.LogError("No Renderer found on reference / reference is null");
        }
        
        OnSetPlayerCurrentDistance();
        _currentXRotation = Math.Clamp(dark + _lightRange * (_currDistance / _distanceRange), dark, bright);
        _deliverXRotation = _currentXRotation;
        OnSetLightDirection();
    }

    private void Update()
    {
        // If locked, we skip updating the rotation
        if (_isLock)
        {
            return;
        }
        OnSetPlayerCurrentDistance();
        _currentXRotation = Math.Clamp(dark + _lightRange * (_currDistance / _distanceRange), dark, bright);
        _deliverXRotation = _currentXRotation;
        OnSetLightDirection();
    }

    // Calculates half the mesh's width (radius)
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

    private void OnSetPlayerCurrentDistance()
    {
        // We take XZ distance from origin
        _currDistance = new Vector2(xrOrigin.position.x, xrOrigin.position.z).magnitude;
    }

    private void OnSetLightDirection()
    {
        // Lerp the x-rotation between dark..bright, proportional to how far the player is from center
        
        directionalLight.localRotation = Quaternion.Euler(
            _currentXRotation,
            directionalLight.localRotation.eulerAngles.y,
            directionalLight.localRotation.eulerAngles.z
        );
    }
    

    /// <summary>
    /// Tells this script to ignore further updates to the directional light in Update().
    /// </summary>
    public void OnSetLock()
    {
        _isLock = true;
        Debug.Log("isLock set to true");
    }

    /// <summary>
    /// Coroutine that lerps x-rotation to 0 over `lightLockAnimDuration` seconds,
    /// following the shape of `lightLockAnimCurve`.
    /// </summary>
    private IEnumerator LockLightingRoutine()
    {
        //Lock Lighting
        OnSetLock();
        float elapsed = 0f;
        // We assume the other angles (Y,Z) remain what they were.
        float startX = _deliverXRotation;

        while (elapsed < lightLockAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / lightLockAnimDuration);

            // Evaluate the curve to get a 0..1 factor
            float curveVal = lightLockAnimCurve.Evaluate(t);

            // Lerp from `startX` to 0
            float currentX = Mathf.LerpAngle(startX, 0f, curveVal);

            // Re-use the existing y,z rotation
            Vector3 eulers = directionalLight.localRotation.eulerAngles;
            eulers.x = currentX;
            directionalLight.localRotation = Quaternion.Euler(eulers);
            _deliverXRotation = currentX;
            yield return null;
        }

        // Snap exactly to x=0
        Vector3 finalEulers = directionalLight.localRotation.eulerAngles;
        finalEulers.x = 0f;
        directionalLight.localRotation = Quaternion.Euler(finalEulers);
        _deliverXRotation = finalEulers.x;
        completeLockLight?.Invoke();
        Destroy(gameObject);
       
    }
}