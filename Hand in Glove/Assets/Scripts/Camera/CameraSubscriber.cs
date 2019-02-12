using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSubscriber : MonoBehaviour {

    private CameraBehaviour cam;
	void Start () {
        cam = FindObjectOfType<CameraBehaviour>();
        if (cam)
            cam.AddObjToFollow(transform);
        else Debug.Log("No Camera set for " + gameObject.name);
    }
}
