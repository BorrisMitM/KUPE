using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour {

    [SerializeField]
    private string sceneName;
	// Update is called once per frame
	void Update () {
        for(int i = 1; i <= 6; i++)
        {
            if (Input.GetButtonDown("Fire2_P" + i))
            {
                GameManager.previousScene = SceneManager.GetActiveScene().name;
                if(GameManager.inBetweenLevels)
                    SceneManager.LoadScene("BetweenLevelSelection");
                else SceneManager.LoadScene("1_MainMenu");
            }
        }
    }
}
