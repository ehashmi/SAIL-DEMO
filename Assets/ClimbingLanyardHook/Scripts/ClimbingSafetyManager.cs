using Autohand;
using UnityEngine;

public class ClimbingSafetyManager : MonoBehaviour
{
    [Header("Hooks")]
    public LanyardHook leftHook;
    public LanyardHook rightHook;

    private Hand leftHand;
    private Hand rightHand;
    private Rigidbody playerRb;

    private ConfigurableJoint activeJoint;
    private LineRenderer ropeRenderer;
    [SerializeField] private bool isHanging = false;

    [Range(0.1f, 10f)]
    public float hangDistance = 0.3f;     // Distance between player & anchor

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();

        // Auto find hands
        Hand[] hands = FindObjectsOfType<Hand>();
        foreach (Hand hand in hands)
        {
            if (hand.left)
                leftHand = hand;
            else
                rightHand = hand;
        }

        // Create rope automatically
        ropeRenderer = gameObject.AddComponent<LineRenderer>();
        ropeRenderer.positionCount = 2;
        ropeRenderer.startWidth = 0.1f;
        ropeRenderer.endWidth = 0.1f;
        ropeRenderer.enabled = false;
        ropeRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void Update()
    {
        CheckFallCondition();
        UpdateRope();
    }

    void CheckFallCondition()
    {
        // bool anyHookConnected = leftHook.IsConnected() || rightHook.IsConnected();

        // // Correct AutoHand grab check
        // bool bothHandsReleased = !leftHand.IsGrabbing() && !rightHand.IsGrabbing();
        // // bool bothHandsReleased = leftHand.holdingObj == null && rightHand.holdingObj == null;

        // if (bothHandsReleased && anyHookConnected)
        // {
        //     if (!isHanging)
        //         StartHanging();
        // }
        // else
        // {
        //     if (isHanging)
        //         StopHanging();
        // }

        bool anyHookConnected = leftHook.IsConnected() || rightHook.IsConnected();
        bool anyHandGrabbing = leftHand.IsGrabbing() || rightHand.IsGrabbing();
        bool bothHandsReleased = !anyHandGrabbing;

        // If grabbing → always climb normally
        if (anyHandGrabbing)
        {
            if (isHanging)
                StopHanging();

            return;
        }

        //Both hands released
        if (bothHandsReleased)
        {
            // If hook connected → hang
            if (anyHookConnected)
            {
                if (!isHanging)
                    StartHanging();
            }
            // If no hook → fall
            else
            {
                if (isHanging)
                    StopHanging();
            }
        }

        // If hanging but hook disconnects
        if (isHanging && !anyHookConnected)
        {
            StopHanging();
        }
        Debug.Log("Left Connected: " + leftHook.IsConnected());
        Debug.Log("Right Connected: " + rightHook.IsConnected());
        Debug.Log("Both Hands Released: " + bothHandsReleased);
    }

    void StartHanging()
    {
        isHanging = true;
        Debug.Log($"Starting to Hang! Left Connected: {leftHook.IsConnected()}, Right Connected: {rightHook.IsConnected()}");

        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        LanyardHook hookToUse = leftHook.IsConnected() ? leftHook : rightHook;
        Rigidbody hookRb = hookToUse.GetComponent<Rigidbody>();

        activeJoint = gameObject.AddComponent<ConfigurableJoint>();
        activeJoint.connectedBody = hookRb;
        activeJoint.autoConfigureConnectedAnchor = false;

        // Anchor setup
        activeJoint.anchor = new Vector3(0, 1.2f, 0);
        activeJoint.connectedAnchor = Vector3.zero;

        // 🔥 IMPORTANT — all Limited
        activeJoint.xMotion = ConfigurableJointMotion.Limited;
        activeJoint.yMotion = ConfigurableJointMotion.Limited;
        activeJoint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = hangDistance;
        activeJoint.linearLimit = limit;

        // Smooth pendulum
        activeJoint.angularXMotion = ConfigurableJointMotion.Free;
        activeJoint.angularYMotion = ConfigurableJointMotion.Free;
        activeJoint.angularZMotion = ConfigurableJointMotion.Free;

        activeJoint.projectionMode = JointProjectionMode.PositionAndRotation;

        ropeRenderer.enabled = true;
    }

    void StopHanging()
    {
        isHanging = false;

        if (activeJoint != null)
            Destroy(activeJoint);
    }

    void UpdateRope()
    {
        LanyardHook activeHook = leftHook.IsConnected() ? leftHook : rightHook;
        if (!isHanging || activeHook == null)
            return;
        Debug.Log($"Updating Rope: Active Hook: {activeHook.name}");
        ropeRenderer.SetPosition(0, activeHook.transform.position);
        ropeRenderer.SetPosition(1, transform.position + Vector3.up * 1.0f);
    }
}
