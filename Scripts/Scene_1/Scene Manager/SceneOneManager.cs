using UnityEngine;
using UnityEngine.Events;

public class SceneOneManager : MonoBehaviour
{
    [Header("Local Events (Scene One)")]
    public UnityEvent playerStartMovement; // Start the player's movement
    public UnityEvent playerReachedDestination; // Handle player reaching the destination
    public UnityEvent playerStageTriggerEnter; // Lock lighting and clean up
    public UnityEvent playerChairTriggerEnter; // Lock and rotate player toward stage

    [Header("Scene Control Parameters")]
    public float waitBeforeMovement = 5f;

    // Trigger player movement after a delay when the scene loads
    private void Start()
    {
        Invoke(nameof(TriggerPlayerStartMovement), waitBeforeMovement);
    }

    // Invokes the movement start event
    private void TriggerPlayerStartMovement()
    {
        playerStartMovement?.Invoke();
    }

    // Invokes destination-reached event
    public void PlayerReachedDestination()
    {
        playerReachedDestination?.Invoke();
    }

    // Invokes stage trigger enter event
    public void PlayerEnteredStageTrigger()
    {
        playerStageTriggerEnter?.Invoke();
    }

    // Invokes chair trigger enter event
    public void PlayerEnteredChairTrigger()
    {
        playerChairTriggerEnter?.Invoke();
    }

    // Signals that the scene is complete via the global event bus
    public void SceneTwoComplete()
    {
        EventBusMain.SceneComplete();
    }
}