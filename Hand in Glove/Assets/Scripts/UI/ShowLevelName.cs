using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShowLevelName : MonoBehaviour {

    private void OnEnable()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.EndsWith("K")) sceneName = sceneName.Remove(sceneName.Length - 1);
        else if (sceneName.EndsWith("PE")) sceneName = sceneName.Remove(sceneName.Length - 2, 2);
        else if (sceneName.EndsWith("KUP")) sceneName = sceneName.Remove(sceneName.Length - 3, 3);
        GetComponent<Text>().text = sceneName;
    }
}
