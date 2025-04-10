using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneEightManager : MonoBehaviour
{
    // Signals that the scene is complete via the global event bus
    public void SceneEightComplete()
    {
        EventBusMain.SceneComplete();
    }
}