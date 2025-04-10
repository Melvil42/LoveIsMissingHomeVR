using System;

public static class EventBusMain
{
    // Global events that any system can subscribe to:
    public static event Action OnSceneComplete;
   

    // Helper methods to invoke events:
    public static void SceneComplete() => OnSceneComplete?.Invoke();
 
}