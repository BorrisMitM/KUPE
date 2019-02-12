using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToLastScene : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i <= 6; i++)
        {
            if (Input.GetButtonDown("Fire2_P" + i))
            {
                SceneManager.LoadScene(GameManager.previousScene);
            }
        }
    }
}
