using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowBestTime : MonoBehaviour {
    Text text;
    string levelName;
    int playerAmount;
    private void Start()
    {
        text = GetComponent<Text>();
        playerAmount = GameManager.playerAmount;
    }
    public void ChangeBestTime(string _levelName)
    {
        levelName = _levelName;
        ChangeBestTime();
    }
    public void ChangeBestTime(int _playerAmount)
    {
        playerAmount = _playerAmount;
        ChangeBestTime();
    }
    private void ChangeBestTime()
    {
        string add = "";
        if (playerAmount == 1) add = "K";
        else if (playerAmount == 2) add = "PE";
        else if (playerAmount == 3) add = "KUP";
        float bestTime = PlayerPrefs.GetFloat(levelName + add + "time");
        if(bestTime == 0f)
        {
            text.text = "~~:~~:~~";
            return;
        }
        int minutes = Mathf.FloorToInt(bestTime / 60f);
        int seconds = Mathf.FloorToInt(bestTime - minutes * 60f);
        float miliseconds = Mathf.FloorToInt((bestTime - Mathf.Floor(bestTime)) * 100f);
        text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }
}
