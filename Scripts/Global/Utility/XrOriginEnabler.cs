using UnityEngine;
using UnityEngine.Events;

public class XrOriginEnabler : MonoBehaviour
{
    public UnityEvent EnableXROrigin;
    public UnityEvent DisableXROrigin;

    public void OnEnableXROrigin()
    {
        EnableXROrigin?.Invoke();
    }

    public void OnDisableXROrigin()
    {
        DisableXROrigin?.Invoke();
    }
}