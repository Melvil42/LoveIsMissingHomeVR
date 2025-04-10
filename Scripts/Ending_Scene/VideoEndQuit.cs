using UnityEngine;
using UnityEngine.Video;

public class VideoEndQuit : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video Finished! Quitting the application.");
        Application.Quit();

        // For editor testing (this won't quit the editor but will stop play mode)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}