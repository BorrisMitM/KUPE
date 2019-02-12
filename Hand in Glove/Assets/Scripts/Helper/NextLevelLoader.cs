using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour {
    public string levelName = "";
	public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadNextLevelWithName()
    {
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(NextLevelLoader))]
public class NextLevelLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NextLevelLoader myScript = (NextLevelLoader)target;
        if (GUILayout.Button("NextLevel"))
        {
            myScript.LoadNextLevel();
        }
    }
}
#endif