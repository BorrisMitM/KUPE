using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    static public bool firstPlay;

    void Awake()
    {
        if (Application.isEditor == false)
        {
            if (PlayerPrefs.GetInt("FirstPlay", 1) == 1)
            {
                firstPlay = true;
                PlayerPrefs.SetInt("FirstPlay", 0);
                SoundManager.SetMasterVolume(1f);
                SoundManager.SetMusicVolume(1f);
                SoundManager.SetSFXVolume(1f);
            }
            else
                firstPlay = false;
        }

    }
}
