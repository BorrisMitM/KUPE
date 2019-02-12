using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Handles the anchor that gets shot out of Karens arm
public class KarenAnchor : MonoBehaviour {

    Rigidbody2D rb;
    RopeAction ropeAction;
    RopeGirlInteraction rGI;
    private PlayerType interactingWith;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
    public void Shoot(Vector2 dir, float speed, float distance, RopeAction _ropeAction)
    {
        ropeAction = _ropeAction;
        GetComponent<Rigidbody2D>().velocity = dir * speed;
        StartCoroutine(DestroyThisIn(distance / speed));  //time till destroy: distance/speed
        Debug.Log(transform.lossyScale);
    }
    IEnumerator DestroyThisIn(float delay)  //stoppable destroy function
    {
        yield return new WaitForSeconds(delay);
        ropeAction.thisAnchor = null;
        Destroy(gameObject);
    }
    public void Shoot(Vector2 dir, float speed, float distance, RopeAction _ropeAction, RopeGirlInteraction _rGI, PlayerType _interactingWith)
    {
        ropeAction = _ropeAction;
        rGI = _rGI;
        interactingWith = _interactingWith;
        GetComponent<Rigidbody2D>().velocity = dir * speed;
        StartCoroutine(DestroyThisInInteraction(distance / speed));  //time till destroy: distance/speed
    }
    IEnumerator DestroyThisInInteraction(float delay)  //stoppable destroy function
    {
        yield return new WaitForSeconds(delay);
        if (interactingWith == PlayerType.ElectroGirl)
        {
            ropeAction.thisAnchor = null;
            rGI.SetEddiArrivedOnDestination();
        }else if(interactingWith == PlayerType.BouncyGuy)
        {
            ropeAction.thisAnchor = null;
            rGI.RopeNotHit();
        }
        interactingWith = PlayerType.None;
        Destroy(gameObject);
    }
    public void AttachToPeter(RopeGirlInteraction _rGI, Transform peterTranform)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rGI = _rGI;
        interactingWith = PlayerType.FlyGuy;
        transform.SetParent(peterTranform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactingWith == PlayerType.ElectroGirl || interactingWith == PlayerType.FlyGuy) return;
        transform.SetParent(collision.transform);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        if (rGI && interactingWith == PlayerType.BouncyGuy)
        {
            rGI.RopeHit(ropeAction.thisAnchor);
            rGI = null;
        }
        else if(ropeAction || (ropeAction && interactingWith == PlayerType.FlyGuy))
        {
            ropeAction.ConnectJoint();
            ropeAction = null;
        }
        interactingWith = PlayerType.None;
        StopAllCoroutines();
        Debug.Log(transform.lossyScale);
    }
}
