using UnityEngine;
using System.Collections;

public class FadeMaterial : MonoBehaviour
{
    // The material attached to this GameObject (retrieved from its Renderer)
    private Material materialToFade;

    [Tooltip("The base color for fading. The RGB channels remain constant while only the alpha is animated.")]
    public Color baseColor = Color.white;

    [Tooltip("AnimationCurve controlling the fade transition. It should be designed for fade out (1 to 0).")]
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 1, 1, 0);

    [Tooltip("Duration of the fade in seconds.")]
    public float fadeDuration = 2.0f;

    [Tooltip("Delay time (in seconds) before starting the fade out.")]
    public float fadeOutDelay = 0.0f;

    [Tooltip("Set to true to fade in (alpha: 0 -> 1) on start, or false to fade out (alpha: 1 -> 0).")]
    public bool fadeInOnStart = true;

    void Start()
    {
        // Grab the Renderer component attached to this GameObject.
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Get a reference to the instance material of this Renderer.
            materialToFade = renderer.material;
        }
        else
        {
            Debug.LogError("No Renderer found on the GameObject!");
            return;
        }

        // Automatically trigger the fade based on the Inspector setting.
        if (fadeInOnStart)
        {
            StartFadeIn();
        }
        else
        {
            StartFadeOut();
        }
    }

    /// <summary>
    /// Initiates the fade-out process (alpha from 1 to 0) with a delay.
    /// </summary>
    public void StartFadeOut()
    {
        if (materialToFade == null)
        {
            Debug.LogError("Material not found. Cannot start fade out.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutWithDelayCoroutine());
    }

    /// <summary>
    /// Coroutine that waits for the specified delay before starting the fade-out.
    /// </summary>
    IEnumerator FadeOutWithDelayCoroutine()
    {
        if (fadeOutDelay > 0f)
        {
            yield return new WaitForSeconds(fadeOutDelay);
        }
        yield return StartCoroutine(FadeCoroutine(false));
    }

    /// <summary>
    /// Initiates the fade-in process (alpha from 0 to 1).
    /// </summary>
    public void StartFadeIn()
    {
        if (materialToFade == null)
        {
            Debug.LogError("Material not found. Cannot start fade in.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(true));
    }

    IEnumerator FadeCoroutine(bool fadeIn)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // For fade-out, the fadeCurve gives the alpha value directly (from 1 to 0).
            // For fade-in, we invert the value to go from 0 to 1.
            float alphaValue = fadeIn ? (1 - fadeCurve.Evaluate(t)) : fadeCurve.Evaluate(t);

            Color newColor = baseColor;
            newColor.a = alphaValue;
            materialToFade.color = newColor;

            yield return null;
        }

        // Ensure the final alpha value is correctly set.
        float finalAlpha = fadeIn ? 1f : 0f;
        Color finalColor = baseColor;
        finalColor.a = finalAlpha;
        materialToFade.color = finalColor;
    }
}
