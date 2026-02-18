using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VRHangingLineSystem : MonoBehaviour
{
    [Header("Top Anchor Points (Ladder / Hook Points)")]
    public Transform topLeftPoint;
    public Transform topRightPoint;

    [Header("Hanging Settings")]
    [Range(0.1f, 10f)]
    public float hangDistance = 2f;     // Distance between player & anchor
    public float ropeWidth = 0.02f;

    private LineRenderer line;
    private ConfigurableJoint joint;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        SetupJoint();
        SetupLine();
    }

    void Update()
    {
        UpdateJointDistance();
        DrawRopes();
    }

    void SetupJoint()
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;

        joint.connectedBody = null;
        joint.connectedAnchor = GetMidPoint();

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = hangDistance;
        joint.linearLimit = limit;
    }

    void UpdateJointDistance()
    {
        if (joint == null) return;

        joint.connectedAnchor = GetMidPoint();

        SoftJointLimit limit = joint.linearLimit;
        limit.limit = hangDistance;
        joint.linearLimit = limit;
    }

    Vector3 GetMidPoint()
    {
        return (topLeftPoint.position + topRightPoint.position) / 2f;
    }

    void SetupLine()
    {
        line.positionCount = 4;
        line.widthMultiplier = ropeWidth;
        line.useWorldSpace = true;
    }

    void DrawRopes()
    {
        if (topLeftPoint == null || topRightPoint == null) return;

        line.SetPosition(0, topLeftPoint.position);
        line.SetPosition(1, transform.position);

        line.SetPosition(2, topRightPoint.position);
        line.SetPosition(3, transform.position);
    }
}
