using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour {
    
	public virtual void TriggerPull(Vector2 dir, float pullForce)
    {
        GetComponent<Rigidbody2D>().velocity = dir * pullForce;
    }
}
