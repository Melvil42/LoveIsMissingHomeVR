using UnityEngine;

/// <summary>
/// Simple script that blends between 2 skybox textures (night/day)
/// based on LightControl’s 0..1 LightProgress.
/// </summary>
[ExecuteAlways]
public class DistanceSkybox : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private LightControl lightControl;

    [Header("Skybox Material & Textures")] [SerializeField]
    private Material blendedSkyboxMaterial; // Must have _Texture1, _Texture2, and _Blend

    [Header("Control Parameters")] [SerializeField]
    private float clampMin;

    [SerializeField] private float clampMax;

    private bool _changeSky = true;

    private void Awake()
    {
        // Optionally set this blendedSkyboxMaterial as the active skybox
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

        // Use the effective rotation for mapping.
        float effectiveProgress = Mathf.InverseLerp(lightControl.dark, lightControl.bright, lightControl.EffectiveRotation);
        // Optionally, apply your MapAndClamp function (if you need additional clamping)
        float t = MapAndClamp(effectiveProgress, clampMin, clampMax, 0, 1);
    
        blendedSkyboxMaterial.SetFloat("_Blend", t);
    }

    static float MapAndClamp(float value, float inMin, float inMax, float outMin, float outMax)
    {
        // If value is below inMin, return outMin
        if (value <= inMin) return outMin;

        // If value is above inMax, return outMax
        if (value >= inMax) return outMax;

        // Map value linearly from the input range to the output range
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }

    public void DestroySkyBoxManager()
    {
        Destroy(gameObject);
    }
}