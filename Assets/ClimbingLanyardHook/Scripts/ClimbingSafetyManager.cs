using Autohand;
using UnityEngine;
using UnityEngine.Events;

public class ClimbingSafetyManager : MonoBehaviour
{
    [Header("Hooks")]
    public LanyardHook leftHook;
    public LanyardHook rightHook;

    private Hand leftHand;
    private Hand rightHand;
    private Rigidbody playerRb;
    [SerializeField] private bool isHanging = false;

    private Hanging hanging;
    public UnityEvent OnHangingStarted;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        hanging = GetComponent<Hanging>();

        // Auto find hands
        Hand[] hands = FindObjectsOfType<Hand>();
        foreach (Hand hand in hands)
        {
            if (hand.left)
                leftHand = hand;
            else
                rightHand = hand;
        }

    }

    void FixedUpdate()
    {
        CheckFallCondition();
        StopHanging();
    }

    void CheckFallCondition()
    {
        bool anyHookConnected = leftHook.IsConnected() || rightHook.IsConnected();
        bool anyHandGrabbing = leftHand.IsGrabbing() || rightHand.IsGrabbing();
        bool bothHandsReleased = !anyHandGrabbing;

        if (anyHookConnected && bothHandsReleased)
        {
            Debug.Log("Both Hands Released: " + bothHandsReleased);
            if (!isHanging)
            {
                StartHanging();
            }
        }

        // // 🧗 If grabbing ladder → climb normally
        // if (anyHandGrabbing)
        // {
        //     if (isHanging)
        //         StopHanging();
        //     return;
        // }

        // // 🪢 Both hands released
        // if (bothHandsReleased)
        // {
        //     if (anyHookConnected)
        //     {
        //         if (!isHanging)
        //             StartHanging();
        //     }
        //     else
        //     {
        //         if (isHanging)
        //             StopHanging();
        //     }
        // }

        // // 💀 Hook disconnected while hanging
        // if (isHanging && !anyHookConnected)
        // {
        //     StopHanging();
        // }
        // Debug.Log("Left Connected: " + leftHook.IsConnected());
        // Debug.Log("Right Connected: " + rightHook.IsConnected());
        // Debug.Log("Both Hands Released: " + bothHandsReleased);
    }

    void StartHanging()
    {
        Debug.Log($"Starting to Hang! Left Connected: {leftHook.IsConnected()}, Right Connected: {rightHook.IsConnected()}");
        isHanging = true;
        OnHangingStarted.Invoke();
        hanging.ResetHanging();
        LanyardHook hookToUse = leftHook.IsConnected() ? leftHook : rightHook;
        hanging.AttachToTwoPoints(hookToUse.transform, playerRb.transform);
    }

    void StopHanging()
    {
        if (!isHanging)
            hanging.Detach();
    }
}
