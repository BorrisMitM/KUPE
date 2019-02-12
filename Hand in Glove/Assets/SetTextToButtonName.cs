using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetTextToButtonName : MonoBehaviour {

	// Use this for initialization
	void Update () {
        GetComponent<Text>().text = transform.parent.gameObject.name;
	}
	
}
