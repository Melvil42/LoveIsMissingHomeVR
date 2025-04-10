using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LookAtPlayerBehaviour : PlayableBehaviour
{
    /// <summary>
    /// The transform of the light (or other object) to be rotated.
    /// </summary>
    public Transform lightTransform;

    /// <summary>
    /// The transform of the player's head (the target).
    /// </summary>
    public Transform playerHeadTransform;

    /// <summary>
    /// An optional curve to control the interpolation (default is linear).
    /// </summary>
    public AnimationCurve interpolationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // Internal state
    private Quaternion initialRotation;
    private bool initialized;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // On first play, capture the initial rotation
        if (lightTransform != null && !initialized)
        {
            initialRotation = lightTransform.rotation;
            initialized = true;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (lightTransform == null || playerHeadTransform == null)
            return;

        // Compute the fraction of time that has passed in the clip.
        double duration = playable.GetDuration();
        double time = playable.GetTime();
        float t = (duration > 0) ? (float)(time / duration) : 0f;
        // Use the interpolation curve to modify the linear t value.
        t = interpolationCurve.Evaluate(t);

        // Compute the target rotation: look from the light toward the player's head.
        // (Here we use Vector3.up as the world up vector.)
        Quaternion targetRotation = Quaternion.LookRotation(
            playerHeadTransform.position - lightTransform.position,
            Vector3.up);

        // Slerp from the initial rotation to the target rotation.
        lightTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
    }

    public override void OnGraphStop(Playable playable)
    {
        // Reset the flag when the graph stops (so that if the clip is played again, we reinitialize)
        initialized = false;
    }
}
