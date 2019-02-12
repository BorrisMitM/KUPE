using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectroGirlInteraction : Interaction {

    [Header("FlyGuy")]
    [SerializeField]
    private float shockVelocity;    //up-velocity of a shock onto flyguy
    [SerializeField]
    private float initialShockMul;  //initial shock is a little bit stronger
    [SerializeField]
    private int charges = 5;        //amount of charges per interaction
    private int currentCharges;
    private float startTolerance = 0.2f;    
    private float toleranceTimer;
    public Transform flyGuyGrabber; //position where eddi grabs fly guy
    private Rigidbody2D flyGuyRB;   //fly guys rigidbody to add velocity of shocks
    public bool onGoingFlyGuyInteraction;
    [SerializeField]
    private Image chargeBar;    //shows the amount of charges left

    private bool onGoingBounceInteraction;
    private Rigidbody2D rb;
    [HideInInspector]
    public bool canShock;
    private DistanceJoint2D joint;
    [Header("Karen")]
    [SerializeField]
    private float shootVelocity;
    [HideInInspector]
    public Transform karenAnchor;
    [HideInInspector]
    public bool arrivedOnDestination = false;
    [HideInInspector]
    public Vector2 shootDir;
    private DashAction dash;
    private List<ParticleSystem> electricalEffect;
    private bool uteAnimOver;
    private Coroutine myCoroutine;
    protected override void Start()
    {
        base.Start();
        thisPlayer = PlayerType.ElectroGirl;
        rb = GetComponent<Rigidbody2D>();
        onGoingFlyGuyInteraction = false;
        onGoingBounceInteraction = false;
        currentCharges = charges;
        charInfo = GetComponent<CharacterInfo>();
        dash = GetComponent<DashAction>();
        electricalEffect = new List<ParticleSystem>();
        foreach (Transform c in transform)              //multiple particles for dash effect
        {
            if (c.gameObject.name.Contains("ElectricalEffect"))
            {
                ParticleSystem ps = c.GetComponent<ParticleSystem>();
                ps.Stop();
                electricalEffect.Add(ps);
            }
        }
    }

    public override bool Interact()// handles interaction start with all other 3 characters
    {
        if (!base.Interact()) return false;
        if (otherInteractor.GetType() == typeof(FlyGuyInteraction)) //disable abilities and initialize interaction
        {
            dash.canDash = false;
            charInfo.canMove = false;
            onGoingFlyGuyInteraction = true;
            GetComponent<Jump>().canJump = false;
            flyGuyRB = otherInteractor.GetComponent<Rigidbody2D>();
            chargeBar.transform.parent.gameObject.SetActive(true);
            SetChargeBar();
            animator.SetTrigger("peter");
            animator.SetBool("bPeter", true);
            myCoroutine = StartCoroutine(InitialShock());
        }
        else if (otherInteractor.GetType() == typeof(BouncyGuyInteraction))
        {
            dash.canDash = false;
            charInfo.canMove = false;
            GetComponent<Jump>().canJump = false;
            onGoingBounceInteraction = true;                        //activate electric particle effect
            foreach (ParticleSystem ps in electricalEffect)
                ps.Play();
            uteAnimOver = false;
            animator.SetTrigger("ute");
            myCoroutine = StartCoroutine(WaitForUteAnimation());
        }
        else if(otherInteractor.GetType() == typeof(RopeGirlInteraction))
        {
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            foreach (ParticleSystem ps in electricalEffect)
                ps.Play();
            foreach (Collider2D c in GetComponents<Collider2D>())       //disable collider to prevent unwanted behaviours when following ute
                c.enabled = false;
            rb.isKinematic = true;
            StartCoroutine(MoveWithKarenAnchor());
        }
        return true;
    }
    IEnumerator WaitForUteAnimation()
    {
        yield return new WaitForSeconds(.35f);
        uteAnimOver = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        foreach (Collider2D c in GetComponents<Collider2D>())       //disable collider to prevent unwanted behaviours when following ute
            c.enabled = false;
        rb.isKinematic = true;
    }
    IEnumerator MoveWithKarenAnchor()
    {
        yield return null;
        while(!arrivedOnDestination)
        {
            transform.position = karenAnchor.position - new Vector3(0f, 1f, 0f);
            yield return null;
        }
        arrivedOnDestination = false;
        rb.isKinematic = false;
        rb.velocity = shootDir * shootVelocity;
        otherInteractor.StopInteract();
        StopInteract();
    }
    public override void DoActionDown()
    {
        if (onGoingFlyGuyInteraction)
        {
            Shock();
            return;
        }
        base.DoActionDown();
        
    }
    public override void DoActionStay()
    {
        base.DoActionStay();
        if (onGoingBounceInteraction && uteAnimOver)       //follow ute during ongoing interaction
            transform.position = (otherInteractor.transform.position) + Vector3.up * .5f;
    }
    public override void DoActionUp()
    {
        base.DoActionUp();
        if (onGoingBounceInteraction)
        {
            otherInteractor.StopInteract();
            StopInteract();
        }
    }
    public override void StopInteract() //resets interactions
    {
        if (onGoingFlyGuyInteraction)
        {
            Destroy(joint);
            charInfo.canMove = true;
            onGoingFlyGuyInteraction = false;
            GetComponent<Jump>().canJump = true;
            currentCharges = charges;
            chargeBar.transform.parent.gameObject.SetActive(false);
            animator.SetBool("bPeter", false);
            StopCoroutine(myCoroutine);
        }
        else if(onGoingBounceInteraction)
        {
            GetComponent<Jump>().canJump = true;
            onGoingBounceInteraction = false;
            StopCoroutine(myCoroutine);
        }
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        foreach (ParticleSystem ps in electricalEffect)
            ps.Stop();
        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = true;
        rb.isKinematic = false;
        dash.canDash = true;
        charInfo.canMove = true;
        GetComponent<InputManager>().active = true;
        base.StopInteract();
    }
    private IEnumerator InitialShock() //connects with flyguy via a joint and launches both into the air
    {
        yield return new WaitForSeconds(.36f);
        if (onGoingInteraction)
        {
            otherInteractor.transform.position = flyGuyGrabber.position;
            joint = gameObject.AddComponent<DistanceJoint2D>();
            joint.connectedBody = flyGuyRB;
            joint.anchor = flyGuyGrabber.localPosition;
            joint.connectedAnchor = Vector2.zero;
            joint.maxDistanceOnly = true;
            flyGuyRB.velocity = new Vector2(flyGuyRB.velocity.x, shockVelocity * initialShockMul);
            toleranceTimer = Time.time + startTolerance;
        }
    }
    private void Shock()    //launches flyguy in the air
    {
        if (currentCharges >= 0 && Time.time >= toleranceTimer)
        {
            currentCharges--;
            flyGuyRB.velocity = new Vector2(flyGuyRB.velocity.x, shockVelocity);
            SetChargeBar();
        }
        else toleranceTimer += Time.deltaTime;
    }

    private void SetChargeBar()
    {
        chargeBar.fillAmount = (float)currentCharges / (float)charges;
    }
    

}
