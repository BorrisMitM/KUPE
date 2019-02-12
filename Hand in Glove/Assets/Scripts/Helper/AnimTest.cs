using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour {
    public bool isRunning;
    public bool jumping;
    public bool grounded;
    Animator animatior;
    // Use this for initialization
    void Start () {
        animatior = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        animatior.SetBool("isRunning", isRunning);
        if (jumping)
        {
            jumping = false;
            animatior.SetTrigger("jumping");
        }
        if (grounded)
        {
            grounded = false;
            animatior.SetTrigger("grounded");
        }
	}
}
