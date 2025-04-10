using UnityEngine;

public class ResetMainCamera : MonoBehaviour
{
    void Start()
    {
        // Find the main camera by tag.
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            // Reset position to (0, 0, 0)
            mainCam.transform.position = Vector3.zero;

            // Reset rotation to (0, 0, 0)
            mainCam.transform.rotation = Quaternion.Euler(0, 0, 0); // or use Quaternion.identity

            // Reset scale to (1, 1, 1)
            mainCam.transform.localScale = Vector3.one;
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Make sure your camera is tagged as 'MainCamera'.");
        }
    }
}