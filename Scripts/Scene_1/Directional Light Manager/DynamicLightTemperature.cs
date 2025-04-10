using UnityEngine;

[ExecuteAlways]
public class DynamicLightTemperature : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DynamicLightControl lightControl;
    [SerializeField] private Light directionalLight; // Requires "Use Color Temperature" enabled

    [Header("Temperature Settings")]
    [SerializeField] private float minKelvin = 3000f;
    [SerializeField] private float maxKelvin = 6500f;

    [Header("Control Parameters")]
    [SerializeField] private float clampMin = 0f;
    [SerializeField] private float clampMax = 1f;

    private bool _changeTemperature = true;

    private void Awake()
    {
        if (directionalLight == null)
        {
            Debug.LogWarning("DistanceLightTemperature: No directional light assigned!");
        }
        if (lightControl == null)
        {
            Debug.LogWarning("DistanceLightTemperature: No LightControl assigned!");
        }
    }

    private void Update()
    {
        if (lightControl == null || directionalLight == null || !_changeTemperature)
            return;

        // Get normalized light progress from LightControl
        float effectiveProgress = lightControl.lightProgress;

        // Clamp and remap progress to 0–1
        float t = MapAndClamp(effectiveProgress, clampMin, clampMax, 0f, 1f);

        // Interpolate between min and max Kelvin values
        float newTemperature = Mathf.Lerp(minKelvin, maxKelvin, t);

        directionalLight.colorTemperature = newTemperature;
    }

    // Maps a value from one range to another with clamping
    static float MapAndClamp(float value, float inMin, float inMax, float outMin, float outMax)
    {
        if (value <= inMin) return outMin;
        if (value >= inMax) return outMax;
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }

    // Destroys this GameObject
    public void DestroyTemperatureController()
    {
        Destroy(gameObject);
    }
}