using UnityEngine;

public class HookAttachPoint : MonoBehaviour
{
    private LanyardHook attachedHook;

    private void OnTriggerEnter(Collider other)
    {
        LanyardHook hook = other.GetComponent<LanyardHook>();

        if (hook != null)
        {
            hook.Connect(this);

            // Snap hook to attach point
            hook.transform.position = transform.position;
            hook.transform.rotation = transform.rotation;

            Debug.Log("Hook Connected: " + hook.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LanyardHook hook = other.GetComponent<LanyardHook>();

        if (hook != null)
        {
            hook.Disconnect();
            Debug.Log("Hook Disconnected: " + hook.name);
        }
    }

    public bool IsOccupied()
    {
        return attachedHook != null;
    }

    public void AttachHook(LanyardHook hook)
    {
        if (hook == null)
            return;

        if (attachedHook != null)
            return; // Already occupied

        attachedHook = hook;

        hook.transform.position = transform.position;
        hook.transform.rotation = transform.rotation;

        hook.Connect(this);

        Debug.Log("Hook Connected: " + hook.name);
    }

    public void ClearHook(LanyardHook hook)
    {
        if (attachedHook == hook)
        {
            attachedHook = null;
        }
    }

    public void DetachCurrentHook()
    {
        if (attachedHook != null)
        {
            attachedHook.currentState = HookConnectionState.Disconnected;
            attachedHook = null;
        }
    }
}
