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
    public float baseAngleOffset = 0f; // Set to 0 if that works best for your setup.
    [Tooltip("The sprite index corresponding to the default sideways orientation.")]
    public int baseSpriteIndex = 9; // Set according to your sprite array.

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
        // Initialize the smoothed angles for each hand.
        if (leftHand.trackedHand != null)
            leftHand.smoothedAngle = NormalizeAngle(leftHand.trackedHand.localEulerAngles.z);
        if (rightHand.trackedHand != null)
            rightHand.smoothedAngle = NormalizeAngle(rightHand.trackedHand.localEulerAngles.z);
    }

    private void Start()
    {
        // Set the default sprite for each hand based on baseSpriteIndex.
        SetSprite(leftHand, leftHand.baseSpriteIndex);
        SetSprite(rightHand, rightHand.baseSpriteIndex);
    }

    private void Update()
    {
        ProcessHandRotation(leftHand);
        ProcessHandRotation(rightHand);
    }

    /// <summary>
    /// Processes the rotation for a given hand and updates the sprite based on turning direction.
    /// </summary>
    private void ProcessHandRotation(HandRotationData hand)
    {
        if (hand.trackedHand == null || hand.spriteRenderer == null || hand.sprites == null || hand.sprites.Length == 0)
        {
            Debug.LogError("Missing component(s) in HandRotationData for " + hand.trackedHand?.name);
            return;
        }

        // Get the target angle and smooth it.
        float targetAngle = NormalizeAngle(hand.trackedHand.localEulerAngles.z);
        hand.smoothedAngle = Mathf.LerpAngle(hand.smoothedAngle, targetAngle, smoothingSpeed * Time.deltaTime);

        // Calculate the signed difference from the base angle.
        float diff = NormalizeAngle(hand.smoothedAngle - hand.baseAngleOffset);
        if (hand.invertRotationDifference)
        {
            diff = -diff;
        }

        // Determine how many steps away from the base orientation.
        int offset = Mathf.FloorToInt(Mathf.Abs(diff) / hand.zAngleThreshold);
        int spriteIndex = diff >= 0 ? hand.baseSpriteIndex + offset : hand.baseSpriteIndex - offset;
        spriteIndex = Mathf.Clamp(spriteIndex, 0, hand.sprites.Length - 1);

        if (spriteIndex != hand.currentSpriteIndex)
        {
            SetSprite(hand, spriteIndex);
        }
    }

    /// <summary>
    /// Updates the hand's sprite and records the current index.
    /// </summary>
    private void SetSprite(HandRotationData hand, int spriteIndex)
    {
        hand.spriteRenderer.sprite = hand.sprites[spriteIndex];
        hand.currentSpriteIndex = spriteIndex;
    }

    /// <summary>
    /// Normalizes an angle from 0–360 to –180 to 180.
    /// </summary>
    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }
}
