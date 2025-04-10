using UnityEngine;


public class HandRotationSpriteSwitcher : MonoBehaviour
{
    [Header("Sprite Settings")]
    public Sprite[] rightHandSprites;
    public Sprite[] leftHandSprites;
    // Array of sprites for different angles
    public GameObject leftHandGameObject;
    public GameObject rightHandGameObject;// SpriteRenderer to display the sprites

    [Header("Rotation Settings")]
    public Transform trackedLeftHand;
    public Transform trackedRightHand;// Reference to the tracked hand (e.g., LeftHand Controller)
    public float leftHandZAngleThreshold = 45f;
    public float rightHandZAngleThreshold = 45f;// Angle threshold per sprite

    private SpriteRenderer _leftHandSpriteRenderer;
    private SpriteRenderer _rightHandSpriteRenderer;
    private int _leftHandCurrentSpriteIndex = -1;
    private int _rightHandCurrentSpriteIndex = -1;// To track the current active sprite

    private void Awake()
    {
        _leftHandSpriteRenderer = leftHandGameObject.GetComponent<SpriteRenderer>();
        _rightHandSpriteRenderer = rightHandGameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Ensure initialization happens when the script is enabled
        if (_leftHandSpriteRenderer != null && leftHandSprites.Length > 0)
        {
            SwitchHandSprite(false, 0, leftHandSprites, _leftHandSpriteRenderer); // Default to the first sprite
        }
        if (_rightHandSpriteRenderer != null && rightHandSprites.Length > 0)
        {
            SwitchHandSprite(true, 0, rightHandSprites, _rightHandSpriteRenderer); // Default to the first sprite
        }
    }

    void Update()
    {
        //left hand update
        if (trackedLeftHand == null || leftHandSprites.Length == 0 || _leftHandSpriteRenderer == null)
            Debug.LogError("Null Reference in left hand model");
        else
        {
            OnChangeRotation(false, trackedLeftHand, leftHandZAngleThreshold, leftHandSprites);
        }
        
        //right hand update
        if (trackedRightHand == null || rightHandSprites.Length == 0 || _rightHandSpriteRenderer == null) 
            Debug.LogError("Null Reference in left hand model");
        else
        {
            OnChangeRotation(true, trackedRightHand, rightHandZAngleThreshold, rightHandSprites);
        }
    }

    private void OnChangeRotation(bool isRight, Transform controller, float angleThreshold, Sprite[] handSpriteArray)
    {
        // Get the current Z-axis rotation
        float rotation = controller.localEulerAngles.z;

        // Normalize the rotation angle (0 to 360 degrees to -180 to 180 degrees)
        if (rotation > 180f) rotation -= 360f;

        // Determine the sprite index based on rotation thresholds
        int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(Mathf.Abs(rotation) / angleThreshold), 0, handSpriteArray.Length - 1);

        // Switch sprite if the index changes
        if (isRight)
        {
            if (spriteIndex != _rightHandCurrentSpriteIndex)
            {
                SwitchHandSprite(true, spriteIndex, rightHandSprites, _rightHandSpriteRenderer);
            }
        }
        else
        {
            if (spriteIndex != _leftHandCurrentSpriteIndex)
            {
                SwitchHandSprite(true, spriteIndex, leftHandSprites, _leftHandSpriteRenderer);
            }
        }
    }

    private void SwitchHandSprite(bool isRight, int newSpriteIndex, Sprite[] handSpritesArray, SpriteRenderer handSpriteRenderer)
    {
        if (newSpriteIndex >= 0 && newSpriteIndex < handSpritesArray.Length)
        {
            handSpriteRenderer.sprite = handSpritesArray[newSpriteIndex];
        }

        UpdateSpriteIndex(newSpriteIndex, isRight);
    }

    private void UpdateSpriteIndex(int index, bool isRight)
    {
        if (isRight)
        {
            _rightHandCurrentSpriteIndex = index;
            return;
        }

        _leftHandCurrentSpriteIndex = index;
    }
    
    private void OnDisable()
    {
        // Optional: Reset the sprite when the script is disabled
        if (_leftHandSpriteRenderer != null)
        {
            _leftHandSpriteRenderer.sprite = null;
        }
        if (_rightHandSpriteRenderer != null)
        {
            _rightHandSpriteRenderer.sprite = null;
        }
        
        _rightHandCurrentSpriteIndex = -1;
        _leftHandCurrentSpriteIndex = -1;
    }
}