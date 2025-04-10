using UnityEngine;

public class StartDisabled : MonoBehaviour
{
    [Tooltip("Disables this GameObject on scene start if enabled.")]
    public bool disableOnStart = true;

    private void Awake()
    {
        if (disableOnStart)
        {
            DisableGameObject();
        }
    }

    // Disables this GameObject
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    // Enables this GameObject
    public void EnableGameObject()
    {
        gameObject.SetActive(true);
    }
}