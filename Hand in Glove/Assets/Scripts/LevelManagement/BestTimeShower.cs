using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BestTimeShower : MonoBehaviour {
    Text text;
	// Use this for initialization
	void OnEnable () {
        text = GetComponent<Text>();
        if (LevelTimes.thisLevelHighscore > 0f)
        {
            LevelTimes.thisLevelHighscore += Time.deltaTime;
            int minutes = Mathf.FloorToInt(LevelTimes.thisLevelHighscore / 60f);
            int seconds = Mathf.FloorToInt(LevelTimes.thisLevelHighscore - minutes * 60f);
            float miliseconds = Mathf.FloorToInt((LevelTimes.thisLevelHighscore - Mathf.Floor(LevelTimes.thisLevelHighscore)) * 100f);
            text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
        }
        else
            text.text = "~~:~~:~~";

    }

}
