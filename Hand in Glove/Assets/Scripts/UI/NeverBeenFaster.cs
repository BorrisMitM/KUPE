using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverBeenFaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!(LevelTimes.thisLevelTime == LevelTimes.thisLevelHighscore))
            gameObject.SetActive(false);
	}

}
