using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnimatePostExposure : MonoBehaviour
{
    [Tooltip("The Volume component to animate.")]
    public Volume volume;

    [Tooltip("Animation curve for the weight animation (normalized time from 0 to 1).")]
    public AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Tooltip("Total duration of the animation in seconds.")]
    public float animationDuration = 2f;

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

    // Destroys this GameObject when a trigger exit occurs
    public void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Animates the volume's weight value from its current state to the target weight
    /// over the specified animation duration using the provided curve.
    /// </summary>
    private IEnumerator AnimateWeight()
    {
        float elapsedTime = 0f;
        float initialWeight = volume.weight;

        while (elapsedTime < animationDuration)
        {
            float normalizedTime = elapsedTime / animationDuration;
            float t = weightCurve.Evaluate(normalizedTime);
            volume.weight = Mathf.Lerp(initialWeight, targetWeight, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        volume.weight = targetWeight;
    }
}