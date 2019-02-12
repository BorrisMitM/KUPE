using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles roll action
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
public class RollAction : Action {
    #region modified attributes
    [SerializeField]
    private float rollMaxVelocity;
    [SerializeField]
    private float rollAcceleration;
    [SerializeField]
    private float rollAirAcceleration;
    [SerializeField]
    public float rollChangeDirAcceleration;
    [SerializeField]
    private float airDamp;
    #endregion 
    #region normal attribues    
    private float normalMaxVelocity;
    private float normalAcceleration;
    private float normalAirAcceleration;
    private float normalChangeDirAcceleration;
    private float normalAirDamp;
    #endregion
    private Movement movement;
    private Jump jump;
    private Animator animator;
    [SerializeField]
    private SkinnedMeshRenderer normalMesh;
    [SerializeField]
    private SkinnedMeshRenderer rollingMesh;
    [SerializeField]
    private Animator rollAnim;

    private void Start()
    {
        movement = GetComponent<Movement>();
        jump = GetComponent<Jump>();
        animator = GetComponentInChildren<Animator>();
        normalMaxVelocity = movement.maxVelocity;
        normalAcceleration = movement.acceleration;
        normalAirAcceleration = movement.airTimeAcceleration;
        normalChangeDirAcceleration = movement.changeDirAcceleration;
        normalAirDamp = movement.airTimeDamping;
    }
    public override void DoActionDown()
    {
        SetRollParameters();
    }
        //Seting the parameters basicly just changes the movement attributes
    public void SetRollParameters()
    {
        movement.maxVelocity = rollMaxVelocity;
        movement.acceleration = rollAcceleration;
        movement.airTimeAcceleration = rollAirAcceleration;
        movement.changeDirAcceleration = rollChangeDirAcceleration;
        movement.airTimeDamping = airDamp;
        movement.acceleratedMove = true;
        jump.canJump = false;
        Transform();
    }
    public void SetRollParameters(float _rollMaxVelocity, float _rollAcceleration, float _rollAirAcceleration, 
                                    float _rollChangeDirAcceleration, float airDamp = 0f)
    {
        movement.maxVelocity = _rollMaxVelocity;
        movement.acceleration = _rollAcceleration;
        movement.airTimeAcceleration = _rollAirAcceleration;
        movement.changeDirAcceleration = _rollChangeDirAcceleration;
        movement.airTimeDamping = airDamp;
        movement.acceleratedMove = true;
        jump.canJump = false;
        Transform();
    }

    public void ResetRollParameters()
    {
        movement.maxVelocity = normalMaxVelocity;
        movement.acceleration = normalAcceleration;
        movement.airTimeAcceleration = normalAirAcceleration;
        movement.changeDirAcceleration = normalChangeDirAcceleration;
        movement.airTimeDamping = normalAirDamp;
        jump.canJump = true;
        TransformBack();
    }
    public override void DoActionStay()
    {

    }
    public override void DoActionUp()
    {
        ResetRollParameters();
    }
    private void Transform()
    {
        normalMesh.enabled = false;
        rollingMesh.enabled = true;
        rollAnim.SetBool("roll", true);
    }
    private void TransformBack()
    {
        normalMesh.enabled = true;
        rollingMesh.enabled = false;
        rollAnim.SetBool("roll", false);
    }
}
