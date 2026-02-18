using UnityEngine;

public class LanyardHook : MonoBehaviour
{
    public bool isConnected;
    public Rigidbody playerRb;

    private ConfigurableJoint joint;

    public void Connect(Rigidbody ladderStep)
    {
        if (isConnected) return;

        joint = playerRb.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ladderStep;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = 1.5f; // rope length
        joint.linearLimit = limit;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;

        isConnected = true;
    }

    public void Disconnect()
    {
        if (!isConnected) return;

        Destroy(joint);
        isConnected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LadderStep"))
        {
            Connect(other.attachedRigidbody);
        }
    }
}
