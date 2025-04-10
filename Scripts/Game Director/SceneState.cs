using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState
{
    SceneOne,
    SceneTwo,
    SceneThree,
    SceneFour,
}

public class Director : MonoBehaviour
{
    public static Director Instance { get; private set; }
    public SceneState CurrentState { get; private set; } = SceneState.SceneOne;

    private void Awake()
    {
        // Ensure only one instance exists and persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set current state based on the active scene
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
        // Subscribe to scene-completion event
        EventBusMain.OnSceneComplete += HandleSceneComplete;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        EventBusMain.OnSceneComplete -= HandleSceneComplete;
    }

    private void HandleSceneComplete()
    {
        // Advance to the next scene based on the current state
        switch (CurrentState)
        {
            case SceneState.SceneOne:
                CurrentState = SceneState.SceneTwo;
                SceneController.instance.LoadScene(1);
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
