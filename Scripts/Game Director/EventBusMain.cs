using System;

public static class EventBusMain
{
    // Global event for signaling when a scene is complete
    public static event Action OnSceneComplete;

    // Method to invoke the scene complete event
    public static void SceneComplete() => OnSceneComplete?.Invoke();
}