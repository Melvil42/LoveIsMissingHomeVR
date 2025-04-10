using UnityEngine;

/// <summary>
/// Simple script that blends between 2 skybox textures (night/day)
/// based on LightControl’s 0..1 LightProgress.
/// </summary>
[ExecuteAlways]
public class SkyBoxTransitionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DynamicLightControl lightControl;

    [Header("Skybox Material & Textures")]
    [SerializeField] private Material blendedSkyboxMaterial; // Requires _Texture1, _Texture2, and _Blend properties

    [Header("Control Parameters")]
    [SerializeField] private float clampMin;
    [SerializeField] private float clampMax;

    private bool _changeSky = true;

    private void Awake()
    {
        if (blendedSkyboxMaterial != null)
        {
            RenderSettings.skybox = blendedSkyboxMaterial;
            blendedSkyboxMaterial.SetFloat("_Blend", 1f);
        }
        else
        {
            Debug.LogWarning("DistanceSkybox: No blendedSkyboxMaterial assigned!");
        }
    }

    private void Update()
    {
        if (lightControl == null || blendedSkyboxMaterial == null || !_changeSky)
            return;

        // Get normalized light rotation progress
        float effectiveProgress = Mathf.InverseLerp(lightControl.dark, lightControl.bright, lightControl.EffectiveRotation);

        // Clamp and remap progress to 0–1 range
        float t = MapAndClamp(effectiveProgress, clampMin, clampMax, 0, 1);

        // Set blend value on skybox material
        blendedSkyboxMaterial.SetFloat("_Blend", t);
    }

    // Maps a value from one range to another with clamping
    static float MapAndClamp(float value, float inMin, float inMax, float outMin, float outMax)
    {
        if (value <= inMin) return outMin;
        if (value >= inMax) return outMax;
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }

    // Destroys this GameObject
    public void DestroySkyBoxManager()
    {
        Destroy(gameObject);
    }
}