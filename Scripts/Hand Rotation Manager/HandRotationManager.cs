using UnityEngine;

[System.Serializable]
public class HandRotationData
{
    [Header("Hand Components")]
    public Transform trackedHand;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    
    [Tooltip("Rotation threshold per sprite (in degrees).")]
    public float zAngleThreshold = 5f;

    [Header("Base Settings")]
    [Tooltip("The angle (in degrees) corresponding to the hand's default sideways orientation.")]
    public float baseAngleOffset = 0f;

    [Tooltip("The sprite index corresponding to the default sideways orientation.")]
    public int baseSpriteIndex = 9;

    [Tooltip("Invert the rotation difference if needed (e.g., true for left hand, false for right hand).")]
    public bool invertRotationDifference = false;

    [HideInInspector] public float smoothedAngle;
    [HideInInspector] public int currentSpriteIndex = -1;
}

public class HandRotationManager : MonoBehaviour
{
    [Header("Hand Settings")]
    public HandRotationData leftHand;
    public HandRotationData rightHand;

    [Header("Smoothing Settings")]
    [Tooltip("Higher values result in less smoothing (faster response)")]
    public float smoothingSpeed = 10f;

    private void Awake()
    {
        // Initialize smoothed angles from the hands' initial rotation
        if (leftHand.trackedHand != null)
            leftHand.smoothedAngle = NormalizeAngle(leftHand.trackedHand.localEulerAngles.z);
        if (rightHand.trackedHand != null)
            rightHand.smoothedAngle = NormalizeAngle(rightHand.trackedHand.localEulerAngles.z);
    }

    private void Start()
    {
        // Set initial sprites based on base index
        SetSprite(leftHand, leftHand.baseSpriteIndex);
        SetSprite(rightHand, rightHand.baseSpriteIndex);
    }

    private void Update()
    {
        // Process hand rotation for each hand
        ProcessHandRotation(leftHand);
        ProcessHandRotation(rightHand);
    }

    private void ProcessHandRotation(HandRotationData hand)
    {
        if (hand.trackedHand == null || hand.spriteRenderer == null || hand.sprites == null || hand.sprites.Length == 0)
        {
            Debug.LogError("Missing component(s) in HandRotationData for " + hand.trackedHand?.name);
            return;
        }

        // Smooth the current Z rotation
        float targetAngle = NormalizeAngle(hand.trackedHand.localEulerAngles.z);
        hand.smoothedAngle = Mathf.LerpAngle(hand.smoothedAngle, targetAngle, smoothingSpeed * Time.deltaTime);

        // Calculate signed angle difference from base orientation
        float diff = NormalizeAngle(hand.smoothedAngle - hand.baseAngleOffset);
        if (hand.invertRotationDifference)
        {
            diff = -diff;
        }

        // Determine sprite index offset from base based on angle threshold
        int offset = Mathf.FloorToInt(Mathf.Abs(diff) / hand.zAngleThreshold);
        int spriteIndex = diff >= 0 ? hand.baseSpriteIndex + offset : hand.baseSpriteIndex - offset;
        spriteIndex = Mathf.Clamp(spriteIndex, 0, hand.sprites.Length - 1);

        // Update sprite if it changed
        if (spriteIndex != hand.currentSpriteIndex)
        {
            SetSprite(hand, spriteIndex);
        }
    }

    // Sets the hand sprite and updates the current index
    private void SetSprite(HandRotationData hand, int spriteIndex)
    {
        hand.spriteRenderer.sprite = hand.sprites[spriteIndex];
        hand.currentSpriteIndex = spriteIndex;
    }

    // Converts angle from 0–360 to -180–180 range
    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }
}
