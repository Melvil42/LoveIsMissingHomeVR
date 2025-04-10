using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance exists and persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load a scene asynchronously by index
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}