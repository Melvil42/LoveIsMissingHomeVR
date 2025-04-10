using UnityEngine;
using UnityEngine.Playables;

public class DestroyTimeline : MonoBehaviour
{
    private PlayableDirector director;

    void Start()
    {
        // Get the PlayableDirector component
        director = GetComponent<PlayableDirector>();

        if (director != null)
        {
            // Subscribe to the stopped event
            director.stopped += OnTimelineStopped;
        }
        else
        {
            Debug.LogWarning("No PlayableDirector component found on this GameObject.");
        }
    }

    private void OnTimelineStopped(PlayableDirector pd)
    {
        // Destroy the GameObject when the timeline finishes
        if (pd == director)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (director != null)
        {
            director.stopped -= OnTimelineStopped;
        }
    }
}