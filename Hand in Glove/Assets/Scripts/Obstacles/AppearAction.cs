using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAction : SwitchAction {
    [SerializeField]
    private bool activeOnStart = false;
	// Use this for initialization
	void Awake () {
         gameObject.SetActive(activeOnStart);
	}

    public override void Do()
    {
        base.Do();
        if(gameObject.activeSelf){
            foreach(Transform c in transform){
                if(c.gameObject.CompareTag("Player"))
                    c.SetParent(null);
            }
        }
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
