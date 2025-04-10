using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState
{
    SceneOne,
    SceneTwo,
    SceneThree,
    SceneFour,
    // Add additional states/scenes as needed.
}

public class Director : MonoBehaviour
{
    public static Director Instance { get; private set; }
    public SceneState CurrentState { get; private set; } = SceneState.SceneOne;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene loads.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Check the active scene and update the state accordingly.
    private void Start()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log("Active scene: " + activeScene.name);

        switch (activeScene.name)
        {
            case "Scene_1":
                CurrentState = SceneState.SceneOne;
                break;
            case "Scene_5":
                CurrentState = SceneState.SceneTwo;
                break;
            case "Scene_8":
                CurrentState = SceneState.SceneThree;
                break;
            case "Ending_Scene":
                CurrentState = SceneState.SceneFour;
                break;
            default:
                Debug.LogWarning("Active scene doesn't match any known scene state.");
                break;
        }
    }

    private void OnEnable()
    {
        // Listen to scene-completion events.
        EventBusMain.OnSceneComplete += HandleSceneComplete;
    }

    private void OnDisable()
    {
        EventBusMain.OnSceneComplete -= HandleSceneComplete;
    }

    private void HandleSceneComplete()
    {
        // Decide what to do when a scene is complete.
        switch (CurrentState)
        {
            case SceneState.SceneOne:
                CurrentState = SceneState.SceneTwo;
                SceneController.instance.LoadScene(1); // Use your actual scene name.
                break;
            case SceneState.SceneTwo:
                CurrentState = SceneState.SceneThree;
                SceneController.instance.LoadScene(2);
                break;
            case SceneState.SceneThree:
                SceneController.instance.LoadScene(3);
                break;
            case SceneState.SceneFour:
                break;
            default:
                Debug.LogWarning("Unhandled scene state in Director.");
                break;
        }
    }
}
