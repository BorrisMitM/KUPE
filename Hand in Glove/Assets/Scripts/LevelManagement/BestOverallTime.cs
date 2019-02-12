using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BestOverallTime : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        float bestTime = PlayerPrefs.GetFloat("bestRunTime", 100000000);
        if (GameManager.doingRun)
        {
            if(LevelTimes.thisRunTime < bestTime)
            {
                bestTime = LevelTimes.thisRunTime;
                PlayerPrefs.SetFloat("bestRunTime", bestTime);
            }
        }
        if (bestTime == 100000000) return;
        int minutes = Mathf.FloorToInt(bestTime / 60f);
        int seconds = Mathf.FloorToInt(bestTime - minutes * 60f);
        float miliseconds = Mathf.FloorToInt((bestTime - Mathf.Floor(bestTime)) * 100f);
        GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }
}
