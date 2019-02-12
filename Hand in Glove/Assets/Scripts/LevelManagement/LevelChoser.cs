using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class LevelChoser : MonoBehaviour,ISelectHandler {
    [HideInInspector]
    public string levelName;
    ScrollRect scrollBar;
    [SerializeField]
    private string sceneToLoad;
    ShowBestTime sbt;
    private void Awake()
    {
        scrollBar = GetComponentInParent<ScrollRect>();
        sbt = FindObjectOfType<ShowBestTime>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        //set the scrollbar position by selected item
        float itemTotal = (float)transform.parent.childCount - 1f;
        float index = (float)transform.GetSiblingIndex();
        scrollBar.verticalNormalizedPosition = 1.0f - (index / itemTotal);
        sbt.ChangeBestTime(levelName);
    }

    public virtual void ChoseLevel()
    {
        GameManager.SetLevelToLoad(levelName);
        GameManager.previousScene = SceneManager.GetActiveScene().name;
        if (GameManager.inBetweenLevels)
            SceneManager.LoadScene(GameManager.GetLevelString());
        else
            SceneManager.LoadScene(sceneToLoad);
    }
}
