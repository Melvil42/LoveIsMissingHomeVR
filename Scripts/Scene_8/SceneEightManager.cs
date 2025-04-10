using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneEightManager : MonoBehaviour
{
    
    public void SceneEightComplete()
    {
        EventBusMain.SceneComplete();
    }
}