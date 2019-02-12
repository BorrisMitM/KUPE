using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {

    //This component provides other components with information about the character
    [Header("Grounded")]
    [SerializeField]
    public LayerMask floorLM;
    public PlayerType playerType;
    public bool grounded;
    [SerializeField]
    public float groundCheckWidth = .3f;
    [SerializeField]
    public float groundCheckHeight = .15f;
    [SerializeField]
    public float groundCheckXOffset = .07f;
    [SerializeField]
    public float groundCheckYOffset = .02f;
    [SerializeField]
    private int rayAmount = 10;
    [SerializeField]
    private bool inSelection = false;
    [Header("Direction")]
    public int dir;
    public Vector2 groundDir;
    public bool canMove;
    public Animator animator;
    private Rigidbody2D rb;
    void Start () {
        dir = 0;
        canMove = true;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        //check if character is grounded
        //Vector2 checkBoxPos = (Vector2)transform.position + new Vector2(0, checkBosOffset);
        //Collider2D col = Physics2D.OverlapArea(checkBoxPos + new Vector2(checkBoxXSize, checkBoxYSize),
        //                                 checkBoxPos + new Vector2(-checkBoxXSize, -checkBoxYSize), floorLM);
        //if (col != null && rb.velocity.y <= 0f)
        //{
        //    grounded = true;
        //    if (col.CompareTag("MovingPlatform")) transform.SetParent(col.transform);
        //}
        //else
        //{
        //    grounded = false;
        //    transform.SetParent(null);
        //}
        bool somethingHit = false;
        bool platformHit = false;
        for(int i = 0; i < rayAmount; i++)
        {
            float xOffset = (float)i / (float)rayAmount * groundCheckWidth - groundCheckWidth / 2f + groundCheckXOffset*dir;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(xOffset, groundCheckYOffset, 0f), Vector3.down, groundCheckHeight, floorLM);
            Debug.DrawRay(transform.position + new Vector3(xOffset, groundCheckYOffset, 0f), Vector3.down * groundCheckHeight, Color.red);
            if (hit)
            {
                grounded = true;
                somethingHit = true;
                if (hit.collider.CompareTag("MovingPlatform"))
                {
                    platformHit = true;
                    transform.SetParent(hit.collider.transform);
                }
            }
        }
        if (!somethingHit) grounded = false;
        if (!platformHit && !inSelection) transform.SetParent(null);
        animator.SetBool("inAir", !grounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
}
