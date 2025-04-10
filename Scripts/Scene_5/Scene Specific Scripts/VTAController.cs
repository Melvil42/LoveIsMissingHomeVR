using UnityEngine;
using System.Collections;

public class VATController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private float _animationTime = 0f;
    private Coroutine _animationCoroutine;

    // Total frames to be used from Houdini export (e.g. 250 out of 500 available)
    public int frameCount = 250;
    // Playback speed in frames per second (adjust this to match your Houdini animation rate)
    public float playbackSpeed = 30f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    // Call this function from your SceneManager or GameDirector to start the animation.
    public void StartVATAnimation()
    {
        _animationTime = 0f;
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        _animationCoroutine = StartCoroutine(AnimateVAT());
    }

    // Coroutine that updates the shader's "Display Frame" property each frame.
    private IEnumerator AnimateVAT()
    {
        while (true)
        {
            // Calculate the current frame based on elapsed time and playback speed.
            // Adding 1 so that frame numbers start at 1.
            int currentFrame = Mathf.FloorToInt(_animationTime * playbackSpeed) + 1;
            
            // Clamp to the maximum frame if exceeded.
            if (currentFrame >= frameCount)
            {
                currentFrame = frameCount;
                UpdateDisplayFrame(currentFrame);
                break;
            }
            
            UpdateDisplayFrame(currentFrame);

            // Increment elapsed time.
            _animationTime += Time.deltaTime;
            yield return null;
        }
        
        // Optionally trigger a callback here to signal the animation is finished.
        _animationCoroutine = null;
    }

    // Updates the shader property "Display Frame" on the material.
    private void UpdateDisplayFrame(int frameValue)
    {
        _renderer.GetPropertyBlock(_propBlock);
        // The shader expects an integer frame number (e.g., 1, 2, 3, …, 250)
        _propBlock.SetFloat("Display Frame", frameValue);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
