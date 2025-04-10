using UnityEngine;
using UnityEngine.Events;

public class XrOriginEnabler : MonoBehaviour
{
    // Events to trigger enabling and disabling the XR Origin
    public UnityEvent EnableXROrigin;
    public UnityEvent DisableXROrigin;

    // Invokes the enable event
    public void OnEnableXROrigin()
    {
        EnableXROrigin?.Invoke();
    }

    // Invokes the disable event
    public void OnDisableXROrigin()
    {
        DisableXROrigin?.Invoke();
    }
}