using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyCheckOrganization : MonoBehaviour {
    public GameObject[] checks;
	// Use this for initialization
	void Start () {
		foreach(InputInformation i in GameManager.inputInformation)
        {
            int inputNr = i.inputNr > GameManager.playerAmount - GameManager.keyBoardPlayersAmount ? i.inputNr - GameManager.keyBoardPlayersAmount - 4 + GameManager.playerAmount : i.inputNr;
            checks[inputNr - 1].SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
