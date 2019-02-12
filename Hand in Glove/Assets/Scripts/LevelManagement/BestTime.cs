using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BestTime : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        int minutes = Mathf.FloorToInt(LevelTimes.thisLevelHighscore / 60f);
        int seconds = Mathf.FloorToInt(LevelTimes.thisLevelHighscore - minutes * 60f);
        float miliseconds = Mathf.FloorToInt((LevelTimes.thisLevelHighscore - Mathf.Floor(LevelTimes.thisLevelHighscore)) * 100f);
        GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }
}
