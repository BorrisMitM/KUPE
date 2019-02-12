using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour {
    private GameObject pausePanel;
	// Use this for initialization
	void Start () {
        pausePanel = transform.Find("PausePanel").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause") && !pausePanel.activeSelf && !FindObjectOfType<CameraBehaviour>().inPlanningPhase)
        {
            Pause();
        }
    }

    public void Pause()
    {
        Camera.main.orthographicSize = 20f;
        Camera.main.GetComponent<Transform>().position = new Vector3(0f, 0f, -10f);
        Time.timeScale = 0f;
        GameManager.paused = true;
        pausePanel.SetActive(true);
        FindObjectOfType<AudioLowPassFilter>().cutoffFrequency = 880f;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        GameManager.paused = false;
        pausePanel.SetActive(false);
        FindObjectOfType<AudioLowPassFilter>().cutoffFrequency = 22000f;
    }
}
