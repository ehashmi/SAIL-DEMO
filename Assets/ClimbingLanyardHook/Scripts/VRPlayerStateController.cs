using UnityEngine;

public enum PlayerState
{
    Grounded,
    Climbing,
    Hanging,
    Falling
}

// attached to player, manages player state based on hand holds and hook connections
public class VRPlayerStateController : MonoBehaviour
{
    public PlayerState currentState;

    public Rigidbody playerRb;
    public LanyardSystem lanyardSystem;

    public bool leftHandHolding;
    public bool rightHandHolding;

    void Update()
    {
        CheckState();
    }

    void CheckState()
    {
        bool noHands = !leftHandHolding && !rightHandHolding;
        bool hookConnected = lanyardSystem.IsAnyHookConnected();

        if (noHands)
        {
            if (hookConnected)
                EnterHanging();
            else
                EnterFalling();
        }
    }

    void EnterHanging()
    {
        if (currentState == PlayerState.Hanging) return;

        currentState = PlayerState.Hanging;

        playerRb.useGravity = true;
        playerRb.linearDamping = 1.5f;   // swing damping
    }

    void EnterFalling()
    {
        if (currentState == PlayerState.Falling) return;

        currentState = PlayerState.Falling;

        playerRb.useGravity = true;
        playerRb.linearDamping = 0f;
    }

    public void EnterClimb()
    {
        currentState = PlayerState.Climbing;
        playerRb.useGravity = false;
    }
}
