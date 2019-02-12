using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//manages the best times and least deaths for the game
public class LevelTimes : MonoBehaviour {
    public static float thisLevelTime;
    public static float thisLevelHighscore;
    public static int deathHighscore;
    public static float thisRunTime;
	// Use this for initialization
	void Start () {
        
	}
    public static void StartNewRun()
    {
        thisRunTime = 0f;
        GameManager.doingRun = true;
        GameManager.levelToLoad = 9;
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //extracts the real level name from the scene loaded, looks up if it is a playable level
        //and loads the appropriate highscores
        string sceneName = scene.name;
        if (sceneName.EndsWith("K")) sceneName = sceneName.Remove(sceneName.Length - 1);
        else if(sceneName.EndsWith("PE")) sceneName = sceneName.Remove(sceneName.Length - 2, 2);
        else if (sceneName.EndsWith("KUP")) sceneName = sceneName.Remove(sceneName.Length - 3, 3);
        if (GameManager.levels.levels.Contains(sceneName))
        {
            thisLevelHighscore = PlayerPrefs.GetFloat(scene.name + "time", 0f);
            deathHighscore = PlayerPrefs.GetInt(scene.name + "deathCount", -1);
        }
    }

    public static void SaveTime(float currentTime)
    {
        //saves highscores 
        thisLevelTime = currentTime;
        if (GameManager.doingRun) thisRunTime += thisLevelTime;
        if (thisLevelTime < thisLevelHighscore || thisLevelHighscore == 0f)
        {
            thisLevelHighscore = currentTime;
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "time", thisLevelHighscore);
        }
        if(Death.deathCount < deathHighscore || deathHighscore == -1)
        {
            deathHighscore = Death.deathCount;
            PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "deathCount", deathHighscore);
        }
    }
}
