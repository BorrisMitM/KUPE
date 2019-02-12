using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlaySavesReset : MonoBehaviour
{
    void Awake()
    {
        if (Application.isEditor == false)
        {
            if (PlayerPrefs.GetInt("FirstPlay", 1) == 1)
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt("FirstPlay", 0);
                PlayerPrefs.Save();
            }
        }
    }
}