using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneButton : MonoBehaviour {
    public void LoadScene(int id)
    {
        SceneLoader.LoadScene(id);
    }
    public void LoadNextScene(int id)
    {
        GameManager.inBetweenLevels = false;
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneLoader.LoadScene(id);
    }
    public void ReloadScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) GameManager.Initialize();
        SceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel()
    {
        GameManager.inBetweenLevels = false;
        GameManager.levelToLoad++;
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(GameManager.GetLevelString());
    }
    public void PreviousLevel()
    {
        GameManager.inBetweenLevels = false;
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(GameManager.GetLevelString());
    }
    public void CharacterSelectionFromInBetween()
    {
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("CharacterSelection");
    }
    public void LevelSelectionFromInBetween()
    {
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LevelSelection");
    }
    public void OptionsFromInBetween()
    {
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Options");
    }
    public void MainMenuFromSplashScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("1_MainMenu");
    }
    public void LevelToLoadFromCutscene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.GetLevelString());
    }
}
