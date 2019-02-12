using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the jump of all characters
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInfo))]
public class Jump : Action {


    [SerializeField]
    private float jumpVelocity = 3f; // normal jump velocity
    private float realJumpVelocity; //current jump velocity
    [SerializeField]
    private float jumpPressSlack = .2f;     //tolerance for pressing jump before touching ground
    private float jumpPressSlackTimer = 0f;
    [SerializeField]
    private float jumpVelocityLoss = .5f;
    public bool canJump = true;
    private Rigidbody2D rb;
    private CharacterInfo charInfo;
    public Animator animator;
    private PlayerSounds playerSounds;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        charInfo = GetComponent<CharacterInfo>();
        animator = GetComponentInChildren<Animator>();
        playerSounds = GetComponent<PlayerSounds>();
        realJumpVelocity = jumpVelocity;
    }
    private void Update()
    {
        if (jumpPressSlackTimer >= 0f) jumpPressSlackTimer -= Time.deltaTime; // jump button pressed?
        if(jumpPressSlackTimer > 0f && charInfo.grounded && canJump)        //grounded and able to jump during slackTimer?
        {
            //handle jump
            charInfo.canMove = false;
            animator.SetTrigger("jumping");
            jumpPressSlackTimer = 0f;
            realJumpVelocity = jumpVelocity;
            StartCoroutine(Jumping());
        }
    }
    IEnumerator Jumping()//small delay for jump to sync with animation
    {
        yield return new WaitForSeconds(0.05f);
        charInfo.canMove = true;
        yield return new WaitForSeconds(0.05f);
        charInfo.canMove = true;
        rb.velocity = new Vector2(rb.velocity.x, realJumpVelocity);
        playerSounds.Jump();
    }
    public override void DoActionDown()
    {
        //give player a little time to press the jump button to early -> responsive feeling
        jumpPressSlackTimer = jumpPressSlack;
    }
    public override void DoActionStay()
    {

    }
    public override void DoActionUp()
    {
        if(rb.velocity.y > 0f) // lower velocity when in air -> better jump feeling
            rb.velocity -= new Vector2(0f, rb.velocity.y * (1f - jumpVelocityLoss));
        if(realJumpVelocity == jumpVelocity)    // lower velocity during jump start animation
            realJumpVelocity = jumpVelocity * jumpVelocityLoss;
    }
}
