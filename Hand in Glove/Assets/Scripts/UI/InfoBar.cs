using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBar : MonoBehaviour {
    
	
	// Update is called once per frame
	void LateUpdate () {
        transform.LookAt(Camera.main.transform);
        if (transform.position.z > 0) transform.position = new Vector3(transform.position.x, transform.position.y, -1);
	}
}
