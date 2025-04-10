using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Change if using a different render pipeline

public class AnimatePostExposure : MonoBehaviour
{
    [Tooltip("The Volume component to animate.")]
    public Volume volume;

    [Tooltip("Animation curve for the weight animation (normalized time from 0 to 1).")]
    public AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Tooltip("Total duration of the animation in seconds.")]
    public float animationDuration = 2f;

    // The target weight value to animate to.
    [Tooltip("Target weight value (for example, 0 to fade out).")]
    public float targetWeight = 0f;

    /// <summary>
    /// Public method to start the volume weight animation.
    /// </summary>
    public void StartAnimation()
    {
        if (volume == null)
        {
            Debug.LogError("Volume is not assigned.");
            return;
        }
        StartCoroutine(AnimateWeight());
    }

    public void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }

    private IEnumerator AnimateWeight()
    {
        float elapsedTime = 0f;
        // Capture the current weight value (assumed to be already set, e.g., 1).
        float initialWeight = volume.weight;

        while (elapsedTime < animationDuration)
        {
            // Calculate normalized time [0, 1].
            float normalizedTime = elapsedTime / animationDuration;
            // Evaluate the curve to get the interpolation factor.
            float t = weightCurve.Evaluate(normalizedTime);
            // Interpolate the weight from the initial value to the target weight.
            volume.weight = Mathf.Lerp(initialWeight, targetWeight, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the final weight is exactly the target value.
        volume.weight = targetWeight;
    }
}