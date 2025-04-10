using UnityEngine;

[ExecuteAlways]
public class DistanceLightTemperature : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LightControl lightControl;
    [SerializeField] private Light directionalLight; // Ensure "Use Color Temperature" is enabled on this light.

    [Header("Temperature Settings")]
    [SerializeField] private float minKelvin = 3000f; // Cold value.
    [SerializeField] private float maxKelvin = 6500f; // Warm value.

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

        // Get the current light progress (0..1) from LightControl.
        float effectiveProgress = lightControl.lightProgress;

        // Optionally remap/clamp the progress.
        float t = MapAndClamp(effectiveProgress, clampMin, clampMax, 0f, 1f);

        // Map the remapped progress to a Kelvin temperature.
        float newTemperature = Mathf.Lerp(minKelvin, maxKelvin, t);

        directionalLight.colorTemperature = newTemperature;
    }

    /// <summary>
    /// Maps a value from one range to another, clamping it to the output range.
    /// </summary>
    static float MapAndClamp(float value, float inMin, float inMax, float outMin, float outMax)
    {
        if (value <= inMin) return outMin;
        if (value >= inMax) return outMax;
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }

    /// <summary>
    /// Call this method to destroy this script's GameObject.
    /// </summary>
    public void DestroyTemperatureController()
    {
        Destroy(gameObject);
    }
}
