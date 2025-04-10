using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class LookAtPlayerAsset : PlayableAsset, ITimelineClipAsset
{
    // This field is used for default values in the inspector.
    public LookAtPlayerBehaviour template = new LookAtPlayerBehaviour();

    // Exposed references allow you to drag & drop scene objects (like your light and the XR camera’s head).
    public ExposedReference<Transform> lightTransform;
    public ExposedReference<Transform> playerHeadTransform;

    // Here we specify what operations this clip supports.
    public ClipCaps clipCaps { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // Create a playable instance from our behaviour.
        var playable = ScriptPlayable<LookAtPlayerBehaviour>.Create(graph, template);
        LookAtPlayerBehaviour behaviour = playable.GetBehaviour();

        // Retrieve the IExposedPropertyTable (usually the PlayableDirector) from the owner GameObject.
        PlayableDirector director = owner.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("PlayableDirector component not found on owner. Please ensure the Timeline has a PlayableDirector.");
            return playable;
        }

        // Resolve the exposed references using the director as the resolver.
        behaviour.lightTransform = lightTransform.Resolve(director);
        behaviour.playerHeadTransform = playerHeadTransform.Resolve(director);
        behaviour.interpolationCurve = template.interpolationCurve;

        return playable;
    }
}