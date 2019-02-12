using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInfo))]
public class Movement : MonoBehaviour {
    
    public float maxVelocity = 5f;
    public float acceleration;
    public float airTimeAcceleration;   
    public float changeDirAcceleration; //acceleration when changing direction
    [Range(0f, 1f)]
    public float groundedDamping;
    [Range(0f, 1f)]
    public float airTimeDamping;
    [SerializeField]
    [Range(0f,1f)]
    private float inputIgnore;  //small inputs get ignored
    private Rigidbody2D rb;
    private CharacterInfo charInfo;
    public Animator animator;
    public bool acceleratedMove = false; //use accelerated move or not.
    private float previousVelocity;
    [HideInInspector]
    public bool damping = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        charInfo = GetComponent<CharacterInfo>();
        animator = GetComponentInChildren<Animator>();

    }
    public void Move(float dir)
    {
        if (acceleratedMove)
            AcceleratedMove(dir);
        else
            AlternateMove(dir);
    }
    //tried alternative way of moving but in the end decided against it 
    public void AlternateMove(float dir)
    {
        if (Mathf.Abs(dir) <= inputIgnore) dir = 0f;
        float horizontalVelocity = 0f;

        if (dir > 0f && charInfo.dir <= 0f) // handle direction of player
        {
            charInfo.dir = 1;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dir < 0f && charInfo.dir >= 0f)
        {
            charInfo.dir = -1;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (charInfo.grounded)                      //acceleration when grounded
        {
            horizontalVelocity = dir * maxVelocity;
            rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        }
        else
        {
            if(Mathf.Abs(rb.velocity.x + dir * airTimeAcceleration) < maxVelocity * 1.2f)
                rb.velocity += new Vector2(dir * airTimeAcceleration, 0f);
        }

        if (Mathf.Abs(horizontalVelocity) > 0 && charInfo.grounded)
            animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        
    }
    //used movement function
    public void AcceleratedMove(float dir)
    {
        if (Mathf.Abs(dir) <= inputIgnore) dir = 0f; // ignrores input below a threshhold
        if (dir > 0f && charInfo.dir <= 0f) // handle direction of player
        {
            charInfo.dir = 1;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dir < 0f && charInfo.dir >= 0f)
        {
            charInfo.dir = -1;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        float horizontalVelocity = rb.velocity.x;

        if (charInfo.grounded)                      //add acceleration when grounded
        {
            if (Mathf.Sign(dir) == Mathf.Sign(rb.velocity.x))
            {
                horizontalVelocity += dir * acceleration * Time.deltaTime;
            }
            else
            {
                horizontalVelocity += dir * changeDirAcceleration * Time.deltaTime; // turning should have a higher deacceleraion
            }
            if(damping) // calculate damping of the movement
                horizontalVelocity *= Mathf.Pow(1f - groundedDamping, Time.deltaTime * 10f);
            //stop the movement on the ground when decreasing and below a specific value
            if (Mathf.Abs(previousVelocity) > Mathf.Abs(horizontalVelocity) && Mathf.Abs(horizontalVelocity) < 4f) horizontalVelocity = 0f;
        }
        else
        {
            horizontalVelocity += dir * airTimeAcceleration * Time.deltaTime;  //different acceleration during airtime
            if(damping)
                horizontalVelocity *= Mathf.Pow(1f - airTimeDamping, Time.deltaTime * 10f);
        }
        if (Mathf.Abs(horizontalVelocity) >= maxVelocity) //don't go above the maximum velocity
            horizontalVelocity = horizontalVelocity > 0f ? maxVelocity : -maxVelocity;

        if (Mathf.Abs(rb.velocity.x) > 1f && charInfo.grounded) // handle run animation
            animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);

        rb.velocity = new Vector2(horizontalVelocity , rb.velocity.y);

        previousVelocity = horizontalVelocity; //used for calculation of velocity after impact with destructable wall
    }
}
