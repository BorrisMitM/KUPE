﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour {
    public float speed = 40f;
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
	}
}
