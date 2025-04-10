using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneFiveManager : MonoBehaviour
{
    public UnityEvent StartBathAnimation;
    
    public void SceneFiveComplete()
    {
        EventBusMain.SceneComplete();
    }

    void Awake()
    {
        // Start the coroutine to wait and then call the method.
        StartCoroutine(CallMethodAfterDelayCoroutine());
    }

    IEnumerator CallMethodAfterDelayCoroutine()
    {
        // Wait for 3 seconds.
        yield return new WaitForSeconds(3f);
    
        // Now call the method.
        StartBathAnimation?.Invoke();
    }

    void CallMethodAfterDelay()
    {
        Debug.Log("Method called after delay");
    }
}


