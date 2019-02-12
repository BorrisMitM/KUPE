using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Defines the character for a player
public enum PlayerType
{
    None,
    FlyGuy,
    BouncyGuy,
    RopeGirl,
    ElectroGirl
}
public class Interaction : Action {

    // This class functions as a base class for the interaction behaviour derived from the action class
    // It handles the playerType setting and getting
    // and handling the syncronization of the button presses of 2 players
    
    protected PlayerType thisPlayer;
    public PlayerType ThisPlayer
    {
        get { return thisPlayer; }
        protected set { }
    }
    protected Interaction otherInteractor;

    protected bool wantToInteract = false;  //only for activating an interaction
    public bool WantToInteract
    {
        get { return wantToInteract; }
        private set { }
    }
    protected bool onGoingInteraction = false;  
    public bool OnGoingInteraction
    {
        get { return onGoingInteraction; }
        private set { }
    }

    [SerializeField]
    private GameObject recognizeParticles;
    private GameObject currentRecognizeParticles;
    public CharacterInfo charInfo;
    protected Animator animator;
    protected virtual void Start () {
        otherInteractor = null;
        animator = GetComponentInChildren<Animator>();
	}

    public override void DoActionDown()
    {
        if(!onGoingInteraction)  //no ongoing interaction?
        {
            wantToInteract = true;      // want to interact
            if(otherInteractor != null)     //available interactor?
            {
                if (otherInteractor.WantToInteract) //wants to interact?
                {
                    onGoingInteraction = true;
                    otherInteractor.onGoingInteraction = true;
                    Interact();                     //lets go
                    if(otherInteractor)             //possible to end the interaction in the first interact
                        otherInteractor.Interact();
                }
            }
        }
    }

    public override void DoActionUp()
    {
        wantToInteract = false;
    }

    public virtual bool Interact()   //implemented in derived classes, bool to let drived classes exit
    {
        if (!onGoingInteraction) return false;  //cannot interact during onGoingInteraction
        return true;
        //onGoingInteraction = true;
    }

    public virtual void StopInteract() 
    {
        onGoingInteraction = false;
        DisconnectInteraction();
    }
    protected virtual void DisconnectInteraction()
    {
        otherInteractor = null;
        GetComponent<PulsateEmission>().pulse = false;
    }

    #region Handling otherInteractions
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Interaction _otherInteractor = collision.GetComponentInParent<Interaction>();
        if (_otherInteractor != null)
        {
            //See if colliding interactor and this interactor is free
            if (otherInteractor == null && _otherInteractor.otherInteractor == null)
            {
                otherInteractor = _otherInteractor;
                otherInteractor.otherInteractor = this;
                ActivateParticles();
                otherInteractor.ActivateParticles();
            }
        }
    }
    //See if interactor in trigger area gets free 
    private void OnTriggerStay2D(Collider2D collision)
    {
        Interaction _otherInteractor = collision.GetComponentInParent<Interaction>();
        if (_otherInteractor != null && otherInteractor == null)
        {
            if(_otherInteractor.otherInteractor == null || _otherInteractor.otherInteractor.ThisPlayer == thisPlayer)   //does other character have no other or this character as interactor?
            {
                otherInteractor = _otherInteractor;
                otherInteractor.otherInteractor = this;
                ActivateParticles();
                otherInteractor.ActivateParticles();
            }
        }
    }
    
    protected void OnTriggerExit2D(Collider2D collision)
    {
        Interaction _otherInteractor = collision.GetComponentInParent<Interaction>();
        if (_otherInteractor != null)
        {
            if(_otherInteractor == otherInteractor && !onGoingInteraction)
            {
                DisconnectInteraction();
            }
        }
    }
    public void ActivateParticles()
    {
        GetComponent<PulsateEmission>().pulse = true;

        //currentRecognizeParticles = Instantiate(recognizeParticles, transform.position + Vector3.up, Quaternion.identity, transform);
        //currentRecognizeParticles.GetComponent<SetParticleColor>().playerType = (otherInteractor.ThisPlayer);
    }
    #endregion
}
