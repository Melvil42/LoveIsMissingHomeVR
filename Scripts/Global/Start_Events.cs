using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Start_Events : MonoBehaviour
{

    public UnityEvent atStart;
    public UnityEvent First_delayedStart;
    public UnityEvent Second_delayedStart;
    public float firstEventTimeDelayed;
    public float secondEventTimeDelayed;

    void Start()
    {
        atStart.Invoke();
        StartCoroutine("First_DelayStart");
        StartCoroutine("Second_DelayStart");
    }

    IEnumerator First_DelayStart()
    {
        yield return new WaitForSeconds(firstEventTimeDelayed);

        First_delayedStart.Invoke();

    }

    IEnumerator Second_DelayStart()
    {
        yield return new WaitForSeconds(secondEventTimeDelayed);

        Second_delayedStart.Invoke();

    }


}
