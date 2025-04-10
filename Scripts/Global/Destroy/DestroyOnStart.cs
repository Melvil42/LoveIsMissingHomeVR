using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }
}
