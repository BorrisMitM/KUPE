using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCheat : MonoBehaviour {

    public GameObject myO;
    float cheatTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            cheatTimer += Time.deltaTime;
        }
        if (cheatTimer >= 1f)
        {
            myO.SetActive(true);
            GameManager.unlockedLevel = 18;
            cheatTimer = 0f;
        }
    }
}
