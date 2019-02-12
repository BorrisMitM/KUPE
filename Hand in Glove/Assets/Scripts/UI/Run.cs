using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Run : MonoBehaviour {

	public void Do()
    {
        LevelTimes.StartNewRun();
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("CharacterSelection");
    }
}
