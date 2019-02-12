using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRunTime : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        float thisRunTime = LevelTimes.thisRunTime;
        int minutes = Mathf.FloorToInt(thisRunTime / 60f);
        int seconds = Mathf.FloorToInt(thisRunTime - minutes * 60f);
        float miliseconds = Mathf.FloorToInt((thisRunTime - Mathf.Floor(thisRunTime)) * 100f);
        GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }
	
}
