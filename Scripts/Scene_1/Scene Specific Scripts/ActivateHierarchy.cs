using UnityEngine;

public class ActivateHierarchy : MonoBehaviour
{
    /// <summary>
    /// Activates this GameObject and all children in the hierarchy.
    /// </summary>
    public void ActivateSelfAndChildren()
    {
        ActivateRecursively(gameObject);
    }

    /// <summary>
    /// Recursively sets the active state of a GameObject and all of its children.
    /// </summary>
    /// <param name="obj">The GameObject to activate.</param>
    private void ActivateRecursively(GameObject obj)
    {
        obj.SetActive(true);
        foreach (Transform child in obj.transform)
        {
            ActivateRecursively(child.gameObject);
        }
    }
}