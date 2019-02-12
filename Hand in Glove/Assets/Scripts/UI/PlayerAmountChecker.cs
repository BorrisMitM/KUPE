using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmountChecker : MonoBehaviour {

    float axisDir;
    Text text;
    ShowBestTime sbt;
    private void Start()
    {
        axisDir = 0;
        text = GetComponent<Text>();
        text.text = GameManager.playerAmount.ToString("0");
        sbt = FindObjectOfType<ShowBestTime>();
    }
    // Update is called once per frame
    void Update () {
        float curAxis = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        if(curAxis != axisDir)
        {
            axisDir = curAxis;
            if(curAxis == 1)
            {
                GameManager.playerAmount++;
            }
            else if (curAxis == -1)
            {
                GameManager.playerAmount--;
            }
            if (GameManager.playerAmount > 4) GameManager.playerAmount = 1;
            else if (GameManager.playerAmount < 1) GameManager.playerAmount = 4;
            sbt.ChangeBestTime(GameManager.playerAmount);
            text.text = GameManager.playerAmount.ToString();
        }
	}
}
