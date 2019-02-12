using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int minutes = Mathf.FloorToInt(LevelTimes.thisLevelTime / 60f);
        int seconds = Mathf.FloorToInt(LevelTimes.thisLevelTime - minutes * 60f);
        float miliseconds = Mathf.FloorToInt((LevelTimes.thisLevelTime - Mathf.Floor(LevelTimes.thisLevelTime)) * 100f);
        GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
	}
	
}
