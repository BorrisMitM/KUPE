using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RollAction))]
public class BouncyGuyInteraction : Interaction {
    [Header("FlyGuy")]
    public Transform flyGuyLauncher;    //position flyguy launches from

    [Header("ElectroGirl")]
    [SerializeField]
    private float rollMaxVelocity;      //movement attributes for electric rolling
    [SerializeField]
    private float rollAcceleration;
    [SerializeField]
    [Range(0, 1)]
    private float rollHorizontalDamp;
    [SerializeField]
    private float rollAirAcceleration;
    private bool onGoingElectroInteraction; 

    [Header("RopeGirl")]
    [SerializeField]
    private bool onGoingRopeInteraction;
    private Vector2 hitPos;
    private float ropeLength;
    private float swingVelocity = 15f;
    private Vector2 swingStartPos;
    private bool swinging;

    //local references
    private Rigidbody2D rb;
    private RollAction rollAction;
    private Movement movement;

    protected override void Start()
    {
        base.Start();
        thisPlayer = PlayerType.BouncyGuy;
        charInfo = GetComponent<CharacterInfo>();
        rb = GetComponent<Rigidbody2D>();
        rollAction = GetComponent<RollAction>();
        movement = GetComponent<Movement>();
        onGoingElectroInteraction = false;
        swinging = false;
    }

    public override bool Interact() // handles interaction start with all other 3 characters
    {
        if (!base.Interact()) return false;
        else if (otherInteractor.GetType() == typeof(ElectroGirlInteraction)) // change movement attributes for faster rolling
        {
            rollAction.SetRollParameters(rollMaxVelocity, 
                                         rollAcceleration, 
                                         rollAirAcceleration, 
                                         0.01f);
            onGoingElectroInteraction = true;
        }
        return true;
    }

    
    public override void DoActionStay()
    {
        base.DoActionStay();
        if (onGoingRopeInteraction) //gets set if ropegirl gets a hit in ropeGirlInteraction
        {
            MoveWithRopeOn();
        }
    }
    
    public override void DoActionUp()
    {
        base.DoActionUp();
        if (onGoingElectroInteraction || onGoingRopeInteraction)
        {
            otherInteractor.StopInteract();
            StopInteract();
        }
    }
    public override void StopInteract() 
    {
        charInfo.canMove = true;
        if (otherInteractor.GetType() == typeof(ElectroGirlInteraction))
        {
            rollAction.ResetRollParameters();
            onGoingElectroInteraction = false;
        }
        else if (otherInteractor.GetType() == typeof(RopeGirlInteraction))
        {
            onGoingRopeInteraction = false;
            swinging = false;
            rollAction.ResetRollParameters();
        }
        base.StopInteract();
    }
    #region RopeInteraction
    public void RopeHit(Vector2 _hitPos)    //gets called from ropeGirlInteraction if something to swing on gets hit
    {
        hitPos = _hitPos;
        ropeLength = ((Vector2)transform.position - hitPos).magnitude;
        rollAction.SetRollParameters();
        rollAction.rollChangeDirAcceleration = 300f;
        charInfo.canMove = false;
        onGoingRopeInteraction = true;
    }
    
    private void MoveWithRopeOn()
    {
        if(((Vector2)transform.position - hitPos).magnitude < ropeLength + 0.1f) //when not stretching the rope
        {
            if (swinging)
                StopSwinging();
            movement.Move(charInfo.dir);        //usual roll movement
        }
        else 
        {
            if(!swinging)
                StartSwing();
            Vector2 hitDir = (hitPos - (Vector2)transform.position).normalized; // direction of the anchor point
            Vector2 velocityDir = hitDir.Rotate(-90f);                          //moving perpendicular to the direction of the anchor point
            rb.velocity = velocityDir * swingVelocity * charInfo.dir;           //tried out joints and self made physicly accurate models
        }                                                                       //but a constant velocity feels the best for this interaction
    }
    private void StopSwinging() //resets parameters for swinging
    {
        swinging = false;
        swingVelocity = 0f;
    }

    private void StartSwing() //sets parameters for swinging
    {
        swinging = true;
        swingStartPos = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision) //interaction with rope girl gets canceled when hitting a wall
    {
        if(!charInfo.grounded && swinging)
        {
            otherInteractor.StopInteract();
            StopInteract();
        }
    }
    #endregion
 

    
}
