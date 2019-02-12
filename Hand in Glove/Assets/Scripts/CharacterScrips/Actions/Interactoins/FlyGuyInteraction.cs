using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyGuyInteraction : Interaction
{
    [Header("BouncyGuy")]
    [SerializeField]
    private float launchSpeed;
    private bool launch = false;

    [Header("ElectroGirl")]
    [HideInInspector]
    public bool onGoingElectroInteraction;
    private FloatAction floatAction;
    private float tolerance = 0.56f;
    private float toleranceTimer;
    private Rigidbody2D rb;
    private bool onGoingRopeGirlInteraction;

    protected override void Start()
    {
        base.Start();
        thisPlayer = PlayerType.FlyGuy;
        rb = GetComponent<Rigidbody2D>();
        onGoingElectroInteraction = false;
        onGoingRopeGirlInteraction = false;
        charInfo = GetComponent<CharacterInfo>();
        floatAction = GetComponent<FloatAction>();
    }

    public override bool Interact() //initializes interactions with other 3 characters
    {
        if (!base.Interact()) return false;
        if (otherInteractor.GetType() == typeof(BouncyGuyInteraction))  //disable abilites and set to position of ute
        {
            otherInteractor.GetComponent<CharacterInfo>().canMove = false;
            otherInteractor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            charInfo.canMove = false;
            launch = true;
            transform.position = otherInteractor.transform.position;
            Launch();
        }
        else if (otherInteractor.GetType() == typeof(ElectroGirlInteraction))
        {
            onGoingElectroInteraction = true;
            floatAction.DoActionDown();
            animator.SetTrigger("eddi");
            animator.SetBool("bEddi", true);
        }
        else if (otherInteractor.GetType() == typeof(RopeGirlInteraction))
        {
            onGoingRopeGirlInteraction = true;
            floatAction.DoActionDown();
        }
        return true;
    }

    public override void DoActionStay()
    {
        if (onGoingElectroInteraction)
        {
            //break interaction after tolerance time
            toleranceTimer += Time.deltaTime;
            if (otherInteractor.GetComponent<CharacterInfo>().grounded && toleranceTimer >= tolerance)
            {
                otherInteractor.StopInteract();
                StopInteract();
            }
            else
            {
                floatAction.DoActionStay(); //Float during interaction
            }
        }
        else if (onGoingRopeGirlInteraction) //float during interaction and draw the rope between flyguy and karen
        {
            RopeGirlInteraction rGI = otherInteractor as RopeGirlInteraction;
            floatAction.DoActionStay();
            if (!charInfo.grounded) rb.mass = 1f;
            else rb.mass = 10f;
        }
    }
    public override void DoActionUp()
    {
        base.DoActionUp();
        if (onGoingRopeGirlInteraction)
            floatAction.DoActionUp();
        if (otherInteractor != null && otherInteractor.GetType() != typeof(BouncyGuyInteraction))
            otherInteractor.StopInteract();
        StopInteract();
    }


    private void Launch()   //Launches FlyGuy in the air when interacting with Ute
    {
        otherInteractor.StopInteract();
        StopInteract();
        rb.velocity = new Vector2(0f, launchSpeed);
        launch = false;
    }

    public override void StopInteract() //disables all interactions
    {
        if (!onGoingInteraction) return;
        charInfo.canMove = true; //TODO make case senisitive
        if (onGoingElectroInteraction)
        {
            toleranceTimer = 0f;
            onGoingElectroInteraction = false;
            floatAction.DoActionUp();
            animator.SetBool("bEddi", false);
        }
        else if (onGoingRopeGirlInteraction)
            onGoingRopeGirlInteraction = false;
        base.StopInteract();
    }
}
