using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneFiveManager : MonoBehaviour
{
    public UnityEvent StartBathAnimation;

    // Signals that the scene is complete via the global event bus
    public void SceneFiveComplete()
    {
        EventBusMain.SceneComplete();
    }

    private void Awake()
    {
        StartCoroutine(CallMethodAfterDelayCoroutine());
    }

    // Waits for a delay, then invokes the bath animation event
    private IEnumerator CallMethodAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(3f);
        StartBathAnimation?.Invoke();
    }

    // Placeholder method (unused)
    private void CallMethodAfterDelay()
    {
        Debug.Log("Method called after delay");
    }
}