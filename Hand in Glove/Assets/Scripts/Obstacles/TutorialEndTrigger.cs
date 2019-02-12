using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour {
    int playersInTrigger = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InputManager>() != null)
        {
            playersInTrigger++;
            if (playersInTrigger == GameManager.inputInformation.Count)
                SceneLoader.LoadScene(GameManager.levels.levels[GameManager.levelToLoad]);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<InputManager>() != null)
            playersInTrigger--;
    }
}
