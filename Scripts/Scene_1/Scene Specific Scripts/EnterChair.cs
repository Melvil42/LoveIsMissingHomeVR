using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterChair : MonoBehaviour
{
    public UnityEvent playerEnteredChair;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Chair");
            playerEnteredChair?.Invoke();
            Destroy(this);
            Debug.Log("EnterChair destroyed");
        }
    }
}
