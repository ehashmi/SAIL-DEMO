using UnityEngine;

//attached to player, manages two hooks for climbing
public class LanyardSystem : MonoBehaviour
{
    public LanyardHook hook1;
    public LanyardHook hook2;

    public bool IsAnyHookConnected()
    {
        return hook1.isConnected || hook2.isConnected;
    }
}
