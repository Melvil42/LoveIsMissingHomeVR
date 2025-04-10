using System.Collections;
using System.Collections.Generic;
using AtmosphericHeightFog;
using UnityEngine;

public class FogAnimationManager : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private LightControl lightControl;

    [SerializeField] private HeightFogGlobal heightFogGlobal;
    
    [Header("Control Parameters")] [SerializeField]
    private float clampMin;

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
        if (lightControl == null || heightFogGlobal== null) return;

        // LightProgress goes from 0 (dark) -> 1 (bright)
        float t = MapAndClamp(lightControl.lightProgress, clampMin, clampMax, 0, 1);

        // Now blend from night -> day
        heightFogGlobal.timeOfDay = 1 - t;
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



}
