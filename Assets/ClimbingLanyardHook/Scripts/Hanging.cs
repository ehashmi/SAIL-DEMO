using UnityEngine;
public class Hanging : MonoBehaviour
{
    [Header("Hanging Settings")]
    [Range(0.1f, 10f)]
    public float hangDistance = 1f;

    [Range(0.01f, 0.5f)]
    public float ropeWidth = 0.02f;

    public bool autoUpdateDistance = true;

    private LineRenderer line;
    private ConfigurableJoint joint;

    private Transform anchorA;
    private Transform anchorB;

    [SerializeField] private bool isHanging = false;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        SetupLine();
    }

    void Update()
    {
        if (!isHanging) return;

        if (autoUpdateDistance)
            UpdateJointDistance();

        DrawRope();
    }

    #region PUBLIC METHODS (Use Anywhere)

    public void AttachToSinglePoint(Transform anchor)
    {
        anchorA = anchor;
        anchorB = null;

        CreateJoint(anchor.position);
        isHanging = true;
    }

    public void AttachToTwoPoints(Transform pointA, Transform pointB)
    {
        anchorA = pointA;
        anchorB = pointB;

        CreateJoint(GetMidPoint());
        isHanging = true;
    }

    public void Detach()
    {
        if (joint != null)
            Destroy(joint);

        line.positionCount = 0;
        isHanging = false;
    }

    #endregion

    void CreateJoint(Vector3 anchorPosition)
    {
        if (joint != null)
            Destroy(joint);

        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;

        joint.connectedBody = null; // World space
        joint.connectedAnchor = anchorPosition;

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

        joint.connectedAnchor = anchorB == null
            ? anchorA.position
            : GetMidPoint();

        SoftJointLimit limit = joint.linearLimit;
        limit.limit = hangDistance;
        joint.linearLimit = limit;
    }

    Vector3 GetMidPoint()
    {
        return (anchorA.position + anchorB.position) / 2f;
    }

    void SetupLine()
    {
        line.useWorldSpace = true;
        line.widthMultiplier = ropeWidth;
    }

    void DrawRope()
    {
        if (anchorA == null) return;

        if (anchorB == null)
        {
            line.positionCount = 2;
            line.SetPosition(0, anchorA.position);
            line.SetPosition(1, transform.position);
        }
        else
        {
            line.positionCount = 4;

            line.SetPosition(0, anchorA.position);
            line.SetPosition(1, transform.position);

            line.SetPosition(2, anchorB.position);
            line.SetPosition(3, transform.position);
        }
    }
}
