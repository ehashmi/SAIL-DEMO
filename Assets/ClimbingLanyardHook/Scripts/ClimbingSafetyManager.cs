using Autohand;
using UnityEngine;

public class ClimbingSafetyManager : MonoBehaviour
{
    [Header("Hooks")]
    public LanyardHook leftHook;
    public LanyardHook rightHook;

    [Header("AutoHand References")]
    public Hand leftHand;
    public Hand rightHand;

    [Header("Player Rigidbody")]
    public Rigidbody playerRb;

    private bool isHanging = false;

    void Awake()
    {
        // 🔥 Auto find Rigidbody
        if (playerRb == null) playerRb = GetComponent<Rigidbody>();

        // 🔥 Auto find both hands in scene
        Hand[] hands = FindObjectsOfType<Hand>();

        foreach (Hand hand in hands)
        {
            if (hand.left)        // AutoHand has bool left
                leftHand = hand;
            else
                rightHand = hand;
        }

        if (leftHand == null || rightHand == null)
            Debug.LogError("Hands not found automatically!");
    }

    void Update()
    {
        CheckFallCondition();
    }

    void CheckFallCondition()
    {
        bool leftConnected = leftHook != null && leftHook.IsConnected();
        bool rightConnected = rightHook != null && rightHook.IsConnected();

        bool anyHookConnected = leftConnected || rightConnected;

        // 🔥 Correct AutoHand grab check
        bool bothHandsReleased = !leftHand.IsGrabbing() && !rightHand.IsGrabbing();
        // bool bothHandsReleased = leftHand.holdingObj == null && rightHand.holdingObj == null;

        if (bothHandsReleased)
        {
            if (anyHookConnected)
            {
                EnableHangingMode();
            }
            else
            {
                EnableFreeFall();
            }
        }
        else
        {
            // If holding ladder again → restore gravity
            playerRb.useGravity = true;
            isHanging = false;
        }
        Debug.Log("Left Connected: " + leftHook.IsConnected());
        Debug.Log("Right Connected: " + rightHook.IsConnected());
        Debug.Log("Both Hands Released: " + bothHandsReleased);
    }

    void EnableHangingMode()
    {
        if (isHanging) return;

        isHanging = true;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.useGravity = false;
    }

    void EnableFreeFall()
    {
        isHanging = false;

        playerRb.useGravity = true;
    }
}
