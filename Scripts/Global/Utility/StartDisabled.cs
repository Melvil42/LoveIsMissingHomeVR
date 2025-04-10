using UnityEngine;

public class StartDisabled : MonoBehaviour
{
    [Tooltip("Should this GameObject be disabled when the scene starts?")]
    public bool disableOnStart = true;

    private void Awake()
    {
        if (disableOnStart)
        {
            DisableGameObject();
        }
    }

    /// <summary>
    /// Disables this GameObject. Can be called externally.
    /// </summary>
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables this GameObject. Can be called externally.
    /// </summary>
    public void EnableGameObject()
    {
        gameObject.SetActive(true);
    }
}