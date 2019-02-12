using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAction : SwitchAction {

	// Use this for initialization
	void Start () {
        GetComponent<MovingPlatform>().active = false;
	}

    public override void Do()
    {
        base.Do();
        GetComponent<MovingPlatform>().active = true;
    }
}
