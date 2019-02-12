using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallTime : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        int minutes = Mathf.FloorToInt(LevelTimes.thisRunTime / 60f);
        int seconds = Mathf.FloorToInt(LevelTimes.thisRunTime - minutes * 60f);
        float miliseconds = Mathf.FloorToInt((LevelTimes.thisRunTime - Mathf.Floor(LevelTimes.thisRunTime)) * 100f);
        GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }
}
