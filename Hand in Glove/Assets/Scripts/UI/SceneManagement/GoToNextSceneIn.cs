using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GoToNextSceneIn : MonoBehaviour {

    [SerializeField]
    private float timeTillNextScene;

    [SerializeField]
    private int id;

    [SerializeField]
    private bool gotoLevelToLoad = false;

    [SerializeField]
    private bool enableClick;

    public bool tryingToSkip = false;
    [SerializeField]
    private GameObject skipDialog;
    [SerializeField]
    private GameObject noButton;

    void Start () {
        StartCoroutine(LoadSceneIn());
	}

    private void Update()
    {
        if (!tryingToSkip)
        {
            if (Input.anyKeyDown && enableClick)
            {
                tryingToSkip = true;
                FindObjectOfType<VideoPlayer>().Pause();
                Time.timeScale = 0f;
                skipDialog.SetActive(true);
                StartCoroutine(SetSelectedObject());
            }
        }
    }
    IEnumerator SetSelectedObject()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        yield return null;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(noButton);
    }
    private IEnumerator LoadSceneIn()
    {
        yield return new WaitForSeconds(timeTillNextScene);
        if (gotoLevelToLoad)
            SceneManager.LoadScene(GameManager.GetLevelString());
        else 
            SceneLoader.LoadScene(id);
    }
    public void Continue()
    {
        Time.timeScale = 1f;
        FindObjectOfType<VideoPlayer>().Play();
        StartCoroutine(ResetTryingToSkip());
        skipDialog.SetActive(false);
    }
    IEnumerator ResetTryingToSkip()
    {
        yield return null;
        tryingToSkip = false;
    }
}
