using UnityEngine;
using System.Collections;

public class SpriteFader : MonoBehaviour
{
    public GameObject spriteRendererLeft;
    public GameObject spriteRendererRight;
    public float duration = 3f;
    private SpriteRenderer _spriteRendererLeft;
    private SpriteRenderer _spriteRendererRight;

    void Awake()
    {
        // Cache the SpriteRenderer components.
        if (spriteRendererLeft == null || spriteRendererRight == null)
        {
            Debug.LogError("SpriteFader requires valid references for spriteRendererLeft and spriteRendererRight.");
        }
        else
        {
            _spriteRendererLeft = spriteRendererLeft.GetComponent<SpriteRenderer>();
            _spriteRendererRight = spriteRendererRight.GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Fades the sprite in by interpolating its alpha to 1 over the given duration.
    /// </summary>
    public void FadeIn()
    {
        spriteRendererLeft.SetActive(true);
        spriteRendererRight.SetActive(true);
        StartCoroutine(Fade(1f, duration, false));
    }

    /// <summary>
    /// Fades the sprite out by interpolating its alpha to 0 over the given duration.
    /// Once the fade-out is complete, the GameObjects are disabled.
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(Fade(0f, duration, true));
    }

    /// <summary>
    /// Fades the sprites to the target alpha over the duration.
    /// Optionally disables the GameObjects after the fade.
    /// </summary>
    private IEnumerator Fade(float targetAlpha, float fadeDuration, bool disableOnFinish)
    {
        Color startColorLeft = _spriteRendererLeft.color;
        Color startColorRight = _spriteRendererRight.color;
        float startAlphaLeft = startColorLeft.a;
        float startAlphaRight = startColorRight.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlphaLeft = Mathf.Lerp(startAlphaLeft, targetAlpha, elapsed / fadeDuration);
            float newAlphaRight = Mathf.Lerp(startAlphaRight, targetAlpha, elapsed / fadeDuration);
            _spriteRendererLeft.color = new Color(startColorLeft.r, startColorLeft.g, startColorLeft.b, newAlphaLeft);
            _spriteRendererRight.color = new Color(startColorRight.r, startColorRight.g, startColorRight.b, newAlphaRight);
            yield return null;
        }

        // Ensure the target alpha is set exactly.
        _spriteRendererLeft.color = new Color(startColorLeft.r, startColorLeft.g, startColorLeft.b, targetAlpha);
        _spriteRendererRight.color = new Color(startColorRight.r, startColorRight.g, startColorRight.b, targetAlpha);

        // If fading out, disable the GameObjects after fade completes.
        if (disableOnFinish)
        {
            spriteRendererLeft.SetActive(false);
            spriteRendererRight.SetActive(false);
        }
    }
}
