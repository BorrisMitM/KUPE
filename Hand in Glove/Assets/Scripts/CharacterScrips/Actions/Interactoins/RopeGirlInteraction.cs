using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGirlInteraction : Interaction {
    [Header("Electrogirl")]
    [SerializeField]
    private Vector2 eddiPreviewOffset;
    [SerializeField]
    private GameObject eddiPreviewPrefab;
    private GameObject eddiPreview; //preview when eddi and karen able to interact
    private Vector2 eddiSpawnLocation;  //location where eddi come out of the rope
    [Header("BouncyGuy")]
    private RaycastHit2D bgHit;
    private bool onGoingBouncyInteraction;
    private GameObject bouncyGuyPreview;    //previews where the rope will hit when able to interact with ute
    public RopeAction ropeAction;
    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private Transform hitPos;
    private bool onGoingFlyGuyInteraction;

    protected override void Start()
    {
        base.Start();
        thisPlayer = PlayerType.RopeGirl;
        ropeAction = GetComponent<RopeAction>();
        charInfo = GetComponent<CharacterInfo>();
        rb = GetComponent<Rigidbody2D>();
        onGoingBouncyInteraction = false;
    }
    public override bool Interact()//initializes interactions for all 3 characters
    {
        if (!base.Interact()) return false;
        if (otherInteractor.GetType() == typeof(ElectroGirlInteraction))
        {
            otherInteractor.GetComponent<InputManager>().Deactivate();
            ElectroGirlInteraction eGI = (ElectroGirlInteraction)otherInteractor;
            ropeAction.ShootAnchorInteraction(eddiSpawnLocation, this, otherInteractor.ThisPlayer);
            eGI.karenAnchor = ropeAction.thisAnchor.transform;
            eGI.shootDir = new Vector2(charInfo.dir, 1f) / Mathf.Sqrt(2);
        }
        else if (otherInteractor.GetType() == typeof(BouncyGuyInteraction))
        {
            charInfo.canMove = false;
            GetComponent<Jump>().canJump = false;
            transform.position = otherInteractor.transform.position;
            if (bouncyGuyPreview) Destroy(bouncyGuyPreview);
            if (ShootRope(otherInteractor.GetComponent<CharacterInfo>().dir))   //shoots a rope in the direction of ute
            {
                onGoingBouncyInteraction = true;
                ropeAction.ShootAnchorInteraction(new Vector2(otherInteractor.charInfo.dir, 1f) / Mathf.Sqrt(2), this, otherInteractor.ThisPlayer);
                foreach (Collider2D c in GetComponents<Collider2D>())   //attaches to ute
                    c.enabled = false;
                rb.isKinematic = true;
            }
            else
            {
                otherInteractor.StopInteract();
                StopInteract();
            }
        }
        else if (otherInteractor.GetType() == typeof(FlyGuyInteraction))
        {
            ropeAction.GiveAnchorToPeter(this, otherInteractor.transform);
            onGoingFlyGuyInteraction = true;
        }
        ropeAction.canRope = false;
        return true;
    }

    public override void DoActionStay()
    {
        base.DoActionStay();
        if (onGoingBouncyInteraction)   //follow ute and draw rope
        {
            //ropeAction.drawLine.Draw();
            transform.position = otherInteractor.transform.position;
        }
        else if (onGoingFlyGuyInteraction)
        {
            ActivateJointWhenNotGrounded();
        }
    }

    public override void DoActionUp()
    {
        base.DoActionUp();
        if (onGoingBouncyInteraction)
        {
            otherInteractor.StopInteract();
            StopInteract();
        }
        else if (onGoingFlyGuyInteraction)
        {
            if (joint) Destroy(joint);
        }
    }
    public void SetEddiArrivedOnDestination()
    {
        ElectroGirlInteraction eGI = (ElectroGirlInteraction)otherInteractor;
        eGI.arrivedOnDestination = true;
    }
    private Collider2D ShootRope(float _dir, bool destroyHitPos = false)    //shoots a rope and returns the collider it connects with
    {
        bgHit = Physics2D.Raycast(ropeAction.drawLine.transform.position, new Vector2(_dir, 1), ropeAction.ropeLength, ropeAction.ropeMask);
        hitPos = new GameObject("hitPos").transform;    //instantiate empty gameobject at position of hit
        hitPos.position = bgHit.point;
        hitPos.SetParent(bgHit.transform);
        if (destroyHitPos) Destroy(hitPos.gameObject);
        return bgHit.collider;
    }

    //Rope Hit and RopeNotHit get called from KarenAnchor to activate/deactivate the interaction with ute
    public void RopeNotHit()
    {
        otherInteractor.StopInteract();
        StopInteract();
    }

    public void RopeHit(GameObject _anchor)
    {
        BouncyGuyInteraction bGI = (BouncyGuyInteraction)otherInteractor;
        bGI.RopeHit(_anchor.transform.position);
    }
    private void Update() // handles previews
    {
        if(!onGoingInteraction && otherInteractor && otherInteractor.GetType() == typeof(BouncyGuyInteraction) && ShootRope(otherInteractor.GetComponent<CharacterInfo>().dir, true))
        {
            if (bouncyGuyPreview == null) bouncyGuyPreview = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bouncyGuyPreview.transform.position = hitPos.position;
            bouncyGuyPreview.transform.localScale = Vector3.one * 0.25f;
        }
        else if(bouncyGuyPreview != null)
        {
            Destroy(bouncyGuyPreview);
            bouncyGuyPreview = null;
        }
        if(!onGoingInteraction && otherInteractor && otherInteractor.GetType() == typeof(ElectroGirlInteraction))
        {
            RaycastHit2D hit = Physics2D.Raycast(ropeAction.drawLine.transform.position, new Vector2(charInfo.dir, 1), ropeAction.ropeLength, ropeAction.ropeMask);
            Vector2 normal = Vector3.zero;
            Vector2 hitPoint = Vector3.zero;
            if (hit)        //calculate offset for spawnpostion
            {
                normal = hit.normal;
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = (Vector2)ropeAction.drawLine.transform.position + new Vector2(charInfo.dir, 1) *Mathf.Sqrt(12.5f);
                Collider2D rightCol = Physics2D.OverlapCircle(hitPoint + new Vector2(eddiPreviewOffset.x/2f, 0f), eddiPreviewOffset.x / 2f, ropeAction.ropeMask);
                Collider2D leftCol = Physics2D.OverlapCircle(hitPoint - new Vector2(eddiPreviewOffset.x / 2f, 0f), eddiPreviewOffset.x / 2f, ropeAction.ropeMask);
                Collider2D topCol = Physics2D.OverlapCapsule(hitPoint + new Vector2(0f, eddiPreviewOffset.y / 2f), new Vector2(eddiPreviewOffset.x / 2f, eddiPreviewOffset.y / 2f),CapsuleDirection2D.Vertical, 0f, ropeAction.ropeMask);
                Collider2D botCol = Physics2D.OverlapCapsule(hitPoint - new Vector2(0f, eddiPreviewOffset.y / 2f), new Vector2(eddiPreviewOffset.x / 2f, eddiPreviewOffset.y / 2f), CapsuleDirection2D.Vertical, 0f, ropeAction.ropeMask);

                if (rightCol)
                    normal += new Vector2(-1f, 0f);
                if (leftCol)
                    normal += new Vector2(1f, 0f);
                if (topCol)
                    normal += new Vector2(0f, -1f);
                if (botCol)
                    normal += new Vector2(0f, 1f);
            }
            if (eddiPreview == null) eddiPreview = Instantiate(eddiPreviewPrefab, Vector3.zero, Quaternion.Euler(0f, 180f,0f));
            //eddiPreview.transform.localScale = eddiPreviewOffset * 2f;
            eddiSpawnLocation = hitPoint + new Vector2(normal.x * eddiPreviewOffset.x, normal.y * eddiPreviewOffset.y);
            eddiPreview.transform.position = eddiSpawnLocation;
        }
    }
    
    private void ActivateJointWhenNotGrounded() //joint only active when at least one character is in air
    {
        if (!charInfo.grounded) rb.mass = 1f;
        else rb.mass = 10f;
        if (!charInfo.grounded || !otherInteractor.charInfo.grounded)
        {
            if (joint == null)
            {
                joint = gameObject.AddComponent<DistanceJoint2D>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedBody = otherInteractor.GetComponent<Rigidbody2D>();
                joint.anchor = ropeAction.drawLine.transform.localPosition;
                joint.connectedAnchor = Vector2.zero;
                joint.maxDistanceOnly = true;
            }
        }
        else
        {
            if (joint) Destroy(joint);
        }
    }
    
    public override void StopInteract() //disable interactions
    {
        charInfo.canMove = true;
        if (otherInteractor.GetType() == typeof(BouncyGuyInteraction))
        {
            charInfo.canMove = true;
            GetComponent<Jump>().canJump = true;
            onGoingBouncyInteraction = false;
            ropeAction.drawLine.Disable();
            Destroy(ropeAction.thisAnchor);
            ropeAction.thisAnchor = null;
            if (hitPos != null)
                Destroy(hitPos.gameObject);
            rb.velocity = otherInteractor.GetComponent<Rigidbody2D>().velocity;
            if (joint) Destroy(joint);
            
        }
        else if (onGoingFlyGuyInteraction)
        {
            ropeAction.drawLine.Disable();
            rb.mass = 1f;
            onGoingFlyGuyInteraction = false;
            if (joint) Destroy(joint);
            Destroy(ropeAction.thisAnchor);
            ropeAction.thisAnchor = null;
        }
        else if(otherInteractor.GetType() == typeof(ElectroGirlInteraction))
        {
            if (eddiPreview) Destroy(eddiPreview);
        }
        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = true;
        rb.isKinematic = false;
        ropeAction.canRope = true;
        base.StopInteract();
    }

    protected override void DisconnectInteraction()
    {
        base.DisconnectInteraction();
        if (bouncyGuyPreview) Destroy(bouncyGuyPreview);
        if (eddiPreview) Destroy(eddiPreview);
    }

    
}
