using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BestDeathCount : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<Text>().text = LevelTimes.deathHighscore.ToString();
    }

}