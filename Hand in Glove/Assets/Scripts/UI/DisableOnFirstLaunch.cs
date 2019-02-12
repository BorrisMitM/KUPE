using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnFirstLaunch : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        if (GameManager.unlockedLevel < 12)
            gameObject.SetActive(false);
	}
}
