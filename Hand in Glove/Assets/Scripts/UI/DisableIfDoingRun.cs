using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfDoingRun : MonoBehaviour {
    public bool yes = true;
	// Use this for initialization
	void Start () {
        if (yes)
        {
            if (GameManager.doingRun) gameObject.SetActive(false);
            else gameObject.SetActive(true);
        }
        else
        {
            if (GameManager.doingRun) gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
    }
}
