using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SceneOneMng : MonoBehaviour
{
    [FormerlySerializedAs("OnPlayerStartMovement")] [Header("Local Events (Scene One)")]

    public UnityEvent playerStartMovement; //start the player movement
    
    [FormerlySerializedAs("OnPlayerReachedDestination")] public UnityEvent playerReachedDestination; //destroy animation character, enable xr origin, enable stage objects.

    [FormerlySerializedAs("OnPlayerStageTriggerEnter")] public UnityEvent playerStageTriggerEnter; //lighting locked & deleted

    [FormerlySerializedAs("OnPlayerChairTriggerEnter")] public UnityEvent playerChairTriggerEnter;
    
    
    
    [Header("Scene Control Parameters")] public float waitBeforeMovement = 5f;
    //As soon as scene loads
    private void Start()
    {
        Invoke(nameof(TriggerPlayerStartMovement), waitBeforeMovement); // Wait 5 seconds before starting.
    }

    private void TriggerPlayerStartMovement()
    {
        // Fire the local event to start the player's movement.
        playerStartMovement?.Invoke();
    }
    
    // This method might be called by another local script (for instance, from the player animation)
    public void PlayerReachedDestination()
    {
        playerReachedDestination?.Invoke();
        
    }

    public void PlayerEnteredStageTrigger()
    {
        playerStageTriggerEnter?.Invoke();
    }

    public void PlayerEnteredChairTrigger()
    {
        playerChairTriggerEnter?.Invoke();
    }

    public void SceneTwoComplete()
    {
        EventBusMain.SceneComplete();
    }
}
