using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeathCount : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = Death.deathCount.ToString();
	}

}
