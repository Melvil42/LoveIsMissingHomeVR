using UnityEngine;
using UnityEngine.Playables;

public class DestroyTimeline : MonoBehaviour
{
    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        if(director != null)
        {
            // Subscribe to the 'stopped' event of the timeline
            director.stopped += OnTimelineStopped;
        }
        else
        {
            Debug.LogWarning("No PlayableDirector component found on this GameObject.");
        }
    }

    private void OnTimelineStopped(PlayableDirector pd)
    {
        // Ensure we're handling the correct director
        if (pd == director)
        {
            // Destroy the timeline GameObject (or you could destroy only the timeline component if needed)
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid potential memory leaks
        if(director != null)
        {
            director.stopped -= OnTimelineStopped;
        }
    }
}