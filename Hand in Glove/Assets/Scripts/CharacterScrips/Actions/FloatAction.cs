using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Handles float ability
public class FloatAction : Action {
    [SerializeField]
    [Range(0, 1)]
    private float fallModifier; //percentage the player falls less
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 capsuleColliderData;
    private Vector3 normalCCD;
    private CapsuleCollider2D cc2d;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        capsuleColliderData = new Vector3(1.7f, .6f, .7f); // horizontal Collider data
        normalCCD = new Vector3(.6f, 1.7f, 1f); // vertical collider data
        cc2d = GetComponent<CapsuleCollider2D>(); // collider that has to be switched when floating
    }
    public override void DoActionDown()
    {
        animator.SetBool("isGliding", true);
    }
    public override void DoActionStay()
    {
        if (rb.velocity.y < 0f) // apply float effect when falling down
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fallModifier);
            ChangeCollider(true);
        }
        else ChangeCollider(false);
    }
    public override void DoActionUp()
    {
        animator.SetBool("isGliding", false);
        ChangeCollider(false);
    }
    void ChangeCollider(bool horizontal)    //handles collider change
    {
        if (horizontal)
        {
            cc2d.direction = CapsuleDirection2D.Horizontal;
            cc2d.size = capsuleColliderData;
            cc2d.offset = new Vector2 (cc2d.offset.x, capsuleColliderData.z);
        }
        else
        {
            cc2d.direction = CapsuleDirection2D.Vertical;
            cc2d.size = normalCCD;
            cc2d.offset = new Vector2(cc2d.offset.x, normalCCD.z);
        }
    }
}
