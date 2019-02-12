using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAction : Action {

    [SerializeField]
    public float ropeLength;
    [SerializeField]
    public float coolDown;
    [SerializeField]
    public LayerMask ropeMask;
    [SerializeField]
    [Range(0f, 1f)]
    private float velocityTransfer;
    [SerializeField]
    [Range(1f, 10f)]
    private float accelerationDecay;
    [SerializeField]
    private float swingAcceleration;
    [SerializeField]
    private float controlledAcceleration;
    [SerializeField]
    private float pullForce;
    [SerializeField]
    private GameObject anchorPrefab;
    private CharacterInfo charInfo;
    private Transform hitPos;
    private Rigidbody2D hitRb;
    private Vector2 swingStartPos;
    private float distance;
    private float swingVelocity;
    Vector2 velocityDir;
    public DrawLine drawLine;
    private float cdOver;
    private DistanceJoint2D dj2D;
    public bool canRope = true;
    public Animator animator;
    public GameObject thisAnchor;
    private void Start()
    {
        charInfo = GetComponent<CharacterInfo>();
        drawLine = GetComponentInChildren<DrawLine>();
        dj2D = GetComponent<DistanceJoint2D>();
        animator = GetComponentInChildren<Animator>();
    }
    public override void DoActionDown()
    {
        if (!canRope) return;
        //if (Time.time <= cdOver) return;
        //cdOver = Time.time + coolDown;
        ShootAnchor(ropeLength);
    }
    public void ShootAnchor(float _ropeLength)
    {
        animator.SetTrigger("rope");
        animator.SetBool("ropeing", true);
        thisAnchor = Instantiate(anchorPrefab, drawLine.start.position, Quaternion.Euler(0f, 0f, charInfo.dir *  -45f));
        drawLine.SetAnchorPoint(thisAnchor.transform);
        thisAnchor.GetComponent<KarenAnchor>().Shoot(new Vector2(charInfo.dir, 1f) / Mathf.Sqrt(2), 
                                                        25f, 
                                                        _ropeLength, 
                                                        this);
        StartCoroutine(DrawRope());
    }
    public void ShootAnchorInteraction(Vector2 destination, RopeGirlInteraction rGI, PlayerType _interactingWith)
    {
        animator.SetTrigger("rope");
        animator.SetBool("ropeing", true);
        thisAnchor = Instantiate(anchorPrefab, drawLine.start.position, Quaternion.Euler(0f, 0f, charInfo.dir *  -45f));
        drawLine.SetAnchorPoint(thisAnchor.transform);
        thisAnchor.GetComponent<KarenAnchor>().Shoot(new Vector2(charInfo.dir, 1f) / Mathf.Sqrt(2), 
                                                        25f, 
                                                        (destination - (Vector2)drawLine.start.position).magnitude, 
                                                        this, 
                                                        rGI, 
                                                        _interactingWith);
        StartCoroutine(DrawRope());
    }
    public void ShootAnchorInteraction(float _dir, RopeGirlInteraction rGI, PlayerType _interactingWith)
    {
        animator.SetTrigger("rope");
        animator.SetBool("ropeing", true);
        thisAnchor = Instantiate(anchorPrefab, drawLine.start.position, Quaternion.Euler(0f, 0f, charInfo.dir *  -45f));
        drawLine.SetAnchorPoint(thisAnchor.transform);
        thisAnchor.GetComponent<KarenAnchor>().Shoot(new Vector2(charInfo.dir, 1f) / Mathf.Sqrt(2),
                                                        25f,
                                                        ropeLength,
                                                        this,
                                                        rGI,
                                                        _interactingWith);
        StartCoroutine(DrawRope());
    }

    public void GiveAnchorToPeter(RopeGirlInteraction _rGI, Transform peterTransform)
    {
        animator.SetBool("ropeing", true);
        thisAnchor = Instantiate(anchorPrefab, peterTransform.position + Vector3.up, Quaternion.Euler(0f, 0f, charInfo.dir *  -45f));
        thisAnchor.GetComponent<KarenAnchor>().AttachToPeter(_rGI, peterTransform);
        drawLine.SetAnchorPoint(thisAnchor.transform);
        StartCoroutine(DrawRope());
    }
    public void ConnectJoint()
    {
        //attach joint and configure it right
        dj2D.enabled = true;
        dj2D.distance = ((Vector2)thisAnchor.transform.position - (Vector2)drawLine.transform.position).magnitude;
        dj2D.autoConfigureConnectedAnchor = false;
        dj2D.connectedBody = thisAnchor.GetComponent<Rigidbody2D>();
        dj2D.anchor = drawLine.transform.localPosition;
        dj2D.connectedAnchor = Vector2.zero;
        dj2D.maxDistanceOnly = true;
    }
    private IEnumerator DrawRope()
    {
        while (thisAnchor)      //breaks when anchor destroys itself
        {
            drawLine.Draw();
            yield return null;
        }
        StopSwinging();
    }

    public override void DoActionUp()
    {
        StopSwinging();
    }

    private void StopSwinging() //disable joint and hitPosition of joint
    {
        dj2D.enabled = false;
        drawLine.Disable();
        animator.SetBool("ropeing", false);
        if(thisAnchor)
            Destroy(thisAnchor);
        thisAnchor = null;
        StopCoroutine(DrawRope());
    }
}
