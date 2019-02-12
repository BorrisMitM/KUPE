using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelLoaderParent : MonoBehaviour {
    [SerializeField]
    private GameObject levelLoaderButton;
	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().sizeDelta = new Vector2(675f, 80f * GameManager.UnlockedLevel);
        for(int i = 0; i < GameManager.UnlockedLevel; i++)
        {
            GameObject button = Instantiate(levelLoaderButton, transform);
            if (i == 0) FindObjectOfType<EventSystem>().firstSelectedGameObject = button;
            button.GetComponentInChildren<Text>().text = GameManager.levels.levels[i];
            button.GetComponent<LevelChoser>().levelName = GameManager.levels.levels[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
