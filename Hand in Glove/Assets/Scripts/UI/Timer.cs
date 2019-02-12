using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour {
    private Text text;
    public float time;
    public bool active;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        time = 0f;
        text.text = time.ToString();
        active = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            time += Time.deltaTime;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);
            float miliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100f);
            text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
        }
	}
}
