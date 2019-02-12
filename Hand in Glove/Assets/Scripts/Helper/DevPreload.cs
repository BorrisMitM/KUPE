using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DevPreload : MonoBehaviour //always loads the preload scene first to initialize game managers
{
    void Awake()
    {
        GameObject check = GameObject.Find("GameManager");
        if (check == null)
        {
            Debug.Log("To Preload");
            UnityEngine.SceneManagement.SceneManager.LoadScene("00_preload");
        }
    }
}

