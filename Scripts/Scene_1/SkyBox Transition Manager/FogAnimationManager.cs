using System.Collections;
using System.Collections.Generic;
using AtmosphericHeightFog;
using UnityEngine;

public class FogAnimationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DynamicLightControl lightControl;
    [SerializeField] private HeightFogGlobal heightFogGlobal;

    [Header("Control Parameters")]
    [SerializeField] private float clampMin;
    [SerializeField] private float clampMax;

    void Awake()
    {
        if (heightFogGlobal != null)
        {
            heightFogGlobal.timeOfDay = 1;
        }
        else
        {
            Debug.LogWarning("FogAnimationManager: No HeightFogGlobal assigned!");
        }
    }

    void Update()
    {
        if (lightControl == null || heightFogGlobal == null) return;

        // Remap light progress and invert it for fog
        float t = MapAndClamp(lightControl.lightProgress, clampMin, clampMax, 0, 1);
        heightFogGlobal.timeOfDay = 1 - t;
    }

    // Maps a value from one range to another, clamped to output range
    static float MapAndClamp(float value, float inMin, float inMax, float outMin, float outMax)
    {
        if (value <= inMin) return outMin;
        if (value >= inMax) return outMax;
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }
}