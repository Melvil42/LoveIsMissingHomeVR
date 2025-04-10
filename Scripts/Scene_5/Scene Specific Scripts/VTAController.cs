using UnityEngine;
using System.Collections;

public class VATController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private float _animationTime = 0f;
    private Coroutine _animationCoroutine;

    // Total number of frames from VAT export (e.g., 250 out of 500)
    public int frameCount = 250;

    // Playback speed in frames per second
    public float playbackSpeed = 30f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    // Starts the VAT animation coroutine
    public void StartVATAnimation()
    {
        _animationTime = 0f;

        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        _animationCoroutine = StartCoroutine(AnimateVAT());
    }

    // Updates the VAT frame based on elapsed time and playback speed
    private IEnumerator AnimateVAT()
    {
        while (true)
        {
            int currentFrame = Mathf.FloorToInt(_animationTime * playbackSpeed) + 1;

            if (currentFrame >= frameCount)
            {
                currentFrame = frameCount;
                UpdateDisplayFrame(currentFrame);
                break;
            }

            UpdateDisplayFrame(currentFrame);

            _animationTime += Time.deltaTime;
            yield return null;
        }

        _animationCoroutine = null;
    }

    // Sets the "Display Frame" shader property using MaterialPropertyBlock
    private void UpdateDisplayFrame(int frameValue)
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("Display Frame", frameValue);
        _renderer.SetPropertyBlock(_propBlock);
    }
}