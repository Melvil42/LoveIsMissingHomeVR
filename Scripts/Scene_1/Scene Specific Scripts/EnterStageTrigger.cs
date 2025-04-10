using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnterStageTrigger : MonoBehaviour
{
    [SerializeField]
    public UnityEvent playerEnteredStage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Stage Trigger");
            playerEnteredStage?.Invoke();
            Destroy(gameObject);
        }
 
    }
}
