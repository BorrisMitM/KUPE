using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputPlayerLinker : MonoBehaviour {
    private static int currentPanel;
    private static int panelAmount;
    private static bool[] inputNotUsed;
    private int panelNr;
    private int inputNr = -1;
    private int activeCharNr = 0;
    private CameraBehaviour cameraBehaviour;
    private int oldAxisRaw = 0;
    private CharacterPool characterPool;
    private void Awake()
    {
        panelAmount = 0;
        currentPanel = 1;
        inputNotUsed = new[] { true, true, true, true };
    }
    private void Start()
    {
        panelAmount++;
        panelNr = panelAmount;
        cameraBehaviour = GameObject.Find("Main Camera").GetComponent<CameraBehaviour>();
        characterPool = GetComponentInParent<CharacterPool>();
        ShowCurrentCharacter();
        GetComponentInChildren<Text>().text = "Player " + panelNr;
    }
    // Update is called once per frame
    void Update () {
        
		if(panelNr == currentPanel)
        {
            if (inputNr > 0)
            {
                float currentAxisRaw = Input.GetAxisRaw("Horizontal_P" + inputNr);
                if (currentAxisRaw > .9f) currentAxisRaw = 1;                   //make axis input discrete
                else if (currentAxisRaw < -.9f) currentAxisRaw = -1;
                else currentAxisRaw = 0;
                if (oldAxisRaw != currentAxisRaw)
                {
                    oldAxisRaw = Mathf.RoundToInt(currentAxisRaw);
                    SetActiveCharNr();
                    ShowCurrentCharacter();
                }
                if (Input.GetButtonDown("Fire1_P" + inputNr)) LinkInput();
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int iPlus1 = i + 1;
                    if (Input.GetButtonDown("Fire1_P" + iPlus1) && inputNotUsed[i])
                    {
                        inputNr = i + 1;
                        GetComponent<Image>().color = new Color(95f/255f, 205f / 255f, 70f / 255f, 100f / 255f);
                        ShowCurrentCharacter();
                    }
                }
            }
        }
	}

    private void SetActiveCharNr()
    {
        activeCharNr += oldAxisRaw;
        //if (activeCharNr < 0) activeCharNr = characterPool.availableCharacters.Count - 1;                 //wraparound
        //else if (activeCharNr >= characterPool.availableCharacters.Count) activeCharNr = 0;
    }

    //not used anymore
    private void SpawnCharacter()
    {
        GameObject newChar = Instantiate(characterPool.availableCharacters[activeCharNr], cameraBehaviour.GetComponent<Camera>().ScreenToWorldPoint(transform.position) + new Vector3(0f,0f,10f), Quaternion.identity);
       // newChar.GetComponent<InputManager>().InitializeInput(panelNr, inputNr);
        cameraBehaviour.AddObjToFollow(newChar.transform);
        inputNotUsed[inputNr - 1] = false;
        //characterPool.availableCharacters.Remove(characterPool.availableCharacters[activeCharNr]);
        GetComponent<Image>().color = new Color(15f / 255f, 15f / 255f, 15f / 255f, 100f / 255f);
        if (currentPanel == panelAmount) Destroy(transform.parent.gameObject);
        currentPanel++;
    }

    private void LinkInput()
    {
        //GameManager.AddInputInformation(characterPool.availableCharacters[activeCharNr], panelNr, inputNr);
        inputNotUsed[inputNr - 1] = false;
        //characterPool.availableCharacters.Remove(characterPool.availableCharacters[activeCharNr]);
        GetComponent<Image>().color = new Color(15f / 255f, 15f / 255f, 15f / 255f, 100f / 255f);
        if (currentPanel == panelAmount) Destroy(transform.parent.gameObject);
        currentPanel++;
    }
    private void ShowCurrentCharacter()
    {
        GetComponentInChildren<Text>().text = "Player " + panelNr + "\n" +characterPool.availableCharacters[activeCharNr].name;
    }
    private void OnDestroy()
    {
        
    }
}
