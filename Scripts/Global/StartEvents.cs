using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StartEvents : MonoBehaviour
{
    // Events to trigger at different times during Start
    public UnityEvent atStart;
    public UnityEvent firstDelayedStart;
    public UnityEvent secondDelayedStart;

    // Delay durations for the delayed events
    public float firstEventTimeDelayed;
    public float secondEventTimeDelayed;

    void Start()
    {
        // Invoke the immediate start event
        atStart.Invoke();

        // Start delayed events
        StartCoroutine("FirstDelayStart");
        StartCoroutine("SecondDelayStart");
    }

    // Coroutine for the first delayed event
    IEnumerator FirstDelayStart()
    {
        yield return new WaitForSeconds(firstEventTimeDelayed);
        firstDelayedStart.Invoke();
    }

    // Coroutine for the second delayed event
    IEnumerator SecondDelayStart()
    {
        yield return new WaitForSeconds(secondEventTimeDelayed);
        secondDelayedStart.Invoke();
    }
}