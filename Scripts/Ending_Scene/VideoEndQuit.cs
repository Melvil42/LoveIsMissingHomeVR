using UnityEngine;
using UnityEngine.Video;

public class VideoEndQuit : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Subscribe to the video end event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video Finished! Quitting the application.");
        Application.Quit();

        // Stop play mode in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
