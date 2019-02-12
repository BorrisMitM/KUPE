using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the dash ability
public class DashAction : Action {
    [SerializeField]
    private float dashDistance;
    [SerializeField]
    private float dashVelocity;
    [SerializeField]
    private float dashEndVelocity; //velocity the character has when exiting the dash
    [SerializeField]
    private float dashCooldown = 0.2f;
    private float dashEndTime;  //helper to count the dash duration
    [HideInInspector]
    public bool canDash;    //for interaction deactivation
    public bool canDashIntern; // for cooldown
    private bool isDashing;
    private Rigidbody2D rb;
    private CharacterInfo charInfo;
    private bool coolDownOver;
    private Jump jump;
    private List<ParticleSystem> electricalEffect;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        charInfo = GetComponent<CharacterInfo>();
        jump = GetComponent<Jump>();
        canDashIntern = true;
        isDashing = false;
        canDash = true;
        coolDownOver = true;
        electricalEffect = new List<ParticleSystem>();
        foreach(Transform c in transform)
        {
            if(c.gameObject.name.Contains("ElectricalEffect"))
            {
                ParticleSystem ps = c.GetComponent<ParticleSystem>();
                ps.Stop();
                electricalEffect.Add(ps);
            }
        }
    }
    public override void DoActionDown()
    {
        if(canDashIntern && canDash)
            StartCoroutine(Dash());
    }

    private void Update()
    {
        if(charInfo.grounded && !isDashing && !canDashIntern && coolDownOver)
        {
            canDashIntern = true;
        }
    }

    IEnumerator Dash()
    {
        dashEndTime = dashDistance / dashVelocity + Time.time;//calculate time when dash ends
        charInfo.canMove = false;
        canDashIntern = false;
        isDashing = true;
        coolDownOver = false;
        jump.canJump = false;
        GetComponentInChildren<Animator>().SetTrigger("dash");
        GetComponent<Movement>().damping = false;//no damping during dash
        foreach(ParticleSystem ps in electricalEffect)
            ps.Play();
        while(Time.time <= dashEndTime)
        {
            rb.velocity = new Vector2(charInfo.dir, 0f) * dashVelocity;
            yield return null;
        }
        GetComponent<Movement>().damping = true;
        foreach (ParticleSystem ps in electricalEffect)
            ps.Stop();
        isDashing = false;
        charInfo.canMove = true;
        jump.canJump = true;
        rb.velocity = new Vector2(charInfo.dir, 0f) * dashEndVelocity;
        StartCoroutine(CoolDown());
    }
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(dashCooldown);
        coolDownOver = true;
    }
}
