using UnityEngine;

public class LanyardHook : MonoBehaviour
{
    public HookConnectionState currentState = HookConnectionState.Disconnected;

    private HookAttachPoint currentAttachPoint;

    public bool IsConnected()
    {
        return currentState == HookConnectionState.Connected;
    }

    public void Connect(HookAttachPoint point)
    {
        currentAttachPoint = point;
        currentState = HookConnectionState.Connected;
    }

    public void Disconnect()
    {
        if (currentAttachPoint != null)
        {
            currentAttachPoint.ClearHook(this);
            currentAttachPoint = null;
        }

        currentState = HookConnectionState.Disconnected;
    }
}

public enum HookConnectionState
{
    Disconnected,
    Connected
}