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
        // Cache SpriteRenderer components from assigned GameObjects
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

    // Fades in both sprites by interpolating alpha to 1
    public void FadeIn()
    {
        spriteRendererLeft.SetActive(true);
        spriteRendererRight.SetActive(true);
        StartCoroutine(Fade(1f, duration, false));
    }

    // Fades out both sprites by interpolating alpha to 0, then disables them
    public void FadeOut()
    {
        StartCoroutine(Fade(0f, duration, true));
    }

    // Interpolates alpha of both sprites to targetAlpha over fadeDuration
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

        // Ensure exact final alpha
        _spriteRendererLeft.color = new Color(startColorLeft.r, startColorLeft.g, startColorLeft.b, targetAlpha);
        _spriteRendererRight.color = new Color(startColorRight.r, startColorRight.g, startColorRight.b, targetAlpha);

        // Disable objects if fade-out completed
        if (disableOnFinish)
        {
            spriteRendererLeft.SetActive(false);
            spriteRendererRight.SetActive(false);
        }
    }
}
