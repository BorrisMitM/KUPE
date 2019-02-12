using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerChoser : MonoBehaviour {
    public static int playerNr;
    private int myPlayerNr;
    private Text text;
    private string inputHorizontalString;
    private string inputConfirmString;
    private string inputDeselectString;
    int axis;
    private float tolerance = .6f;
    private CharacterPool charPool;
    private int cursorNr;
    private bool axisInUse;
    private static List<LinkInfo> linkInfo;
    private LinkInfo myLinkInfo;
    private bool linked = false;
    private static int lockedIn;
    private bool isLockedIn = false;
    [SerializeField]
    private Color selectColor;
    [SerializeField]
    private Color lockedInColor;

    // Use this for initialization
    void Start () {
        playerNr++;
        myPlayerNr = playerNr;
        cursorNr = myPlayerNr - 1;
        if (GameManager.useKeyboard && myPlayerNr > GameManager.playerAmount - GameManager.keyBoardPlayersAmount) myPlayerNr += GameManager.keyBoardPlayersAmount + 4 - GameManager.playerAmount;
        text = GetComponentInChildren<Text>();
        text.text = "P" + (cursorNr + 1);
        inputHorizontalString = "Horizontal_P" + myPlayerNr;
        inputConfirmString = "Fire1_P" + myPlayerNr;
        inputDeselectString = "Fire2_P" + myPlayerNr;
        axis = 0;
        linkInfo = new List<LinkInfo>();
        charPool = GetComponentInParent<CharacterPool>();
        if(cursorNr == 0)
        {
            foreach (string s in Input.GetJoystickNames())
            Debug.Log(s);
        }
        lockedIn = 0;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.paused) return;
        if (!linked)
        {
            AxisHandler();
            if (axis == 1)
            {
                GoRight();
            }
            else if (axis == -1)
                GoLeft();
            if (Input.GetButtonDown(inputConfirmString))
                LinkPlayerInput();
            else if (Input.GetButtonDown(inputDeselectString))
            {
                SceneManager.LoadScene(GameManager.previousScene);
            }
        }
        else
        {
            if (Input.GetButtonDown(inputDeselectString))
            {
                UnlinkPlayerInput();
            }else if (Input.GetButtonDown(inputConfirmString) && !isLockedIn)
            {
                lockedIn++;
                isLockedIn = true;
                GameManager.inputInformation = new List<InputInformation>();
                foreach (LinkInfo li in linkInfo)
                {
                    GameManager.AddInputInformation(li.cursorNr, li.playerNr);    //make globaly and for every scene known
                    if(GameManager.playerAmount == lockedIn)
                    {
                        LoadLevel();
                    }
                }
                GetComponentInChildren<Text>().color = lockedInColor;
                GetComponentInChildren<Image>().color = lockedInColor;
            }
        }
            
	}

    private static void LoadLevel()
    {
        if (GameManager.inBetweenLevels)
            SceneManager.LoadScene("BetweenLevelSelection");
        else
            SceneManager.LoadScene(GameManager.GetLevelString());
    }

    private void UnlinkPlayerInput()
    {
        isLockedIn = false;
        lockedIn--;
        linked = false;
        linkInfo.Remove(myLinkInfo);
        GetComponentInChildren<Text>().color = Color.white;
        GetComponentInChildren<Image>().color = Color.white;
    }

    private void LinkPlayerInput()
    {
        foreach(LinkInfo li in linkInfo)
        {
            if (li.cursorNr == cursorNr) return;
        }
        myLinkInfo = new LinkInfo(CharacterPool.lowBorder + cursorNr, myPlayerNr);
        linkInfo.Add(myLinkInfo);
        linked = true;
        charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].GetComponent<PlayerSounds>().OneLiner();
        GetComponentInChildren<Text>().color = selectColor;
        GetComponentInChildren<Image>().color = selectColor;
        if (charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].GetComponentInChildren<AnimatorController>())
        charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].GetComponentInChildren<AnimatorController>().SetChosen();
    }

    private void GoLeft()
    {
        charPool.assignArray[cursorNr].Remove(gameObject);
        for (int i = 0; i < charPool.assignArray[cursorNr].Count; i++)
        {
            charPool.assignArray[cursorNr][i].transform.position = charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].transform.position + new Vector3(0f, (float)(i + 1) * charPool.yOffset, 0f);
        }
        for (int i = 0; i < charPool.assignArray.Length; i++)   //search for next available list
        {
            cursorNr--;
            if (cursorNr < 0) cursorNr = GameManager.playerAmount - 1;   //wraparound
            if (charPool.assignArray[cursorNr] != null) break;
        }
        charPool.assignArray[cursorNr].Add(gameObject);
        transform.position = charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].transform.position + new Vector3(0f, charPool.assignArray[cursorNr].Count * charPool.yOffset, 0f);
    }

    public void GoRight()
    {
        charPool.assignArray[cursorNr].Remove(gameObject);
        for(int i = 0; i < charPool.assignArray[cursorNr].Count; i++)
        {
            charPool.assignArray[cursorNr][i].transform.position = charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].transform.position + new Vector3(0f, (float)(i+1) * charPool.yOffset, 0f);
        }
        for (int i = 0; i < charPool.assignArray.Length; i++)   //search for next available list
        {
            cursorNr++;
            if (cursorNr >= GameManager.playerAmount) cursorNr = 0;
            if (charPool.assignArray[cursorNr] != null) break;
        }
        charPool.assignArray[cursorNr].Add(gameObject);
        transform.position = charPool.availableCharacters[CharacterPool.lowBorder + cursorNr].transform.position + new Vector3(0f, charPool.assignArray[cursorNr].Count * charPool.yOffset, 0f);
    }

    private void AxisHandler()
    {
        if(axisInUse)
        {
            axis = 0;
            if(Mathf.Abs(Input.GetAxisRaw(inputHorizontalString)) < tolerance)
                axisInUse = false;
        }
        else if (Input.GetAxisRaw(inputHorizontalString) > tolerance && !axisInUse)
        {
            axis = 1;
            axisInUse = true;
        }
        else if (Input.GetAxisRaw(inputHorizontalString) < -tolerance && !axisInUse)
        {
            axis = -1;
            axisInUse = true;
        }
    }

    struct LinkInfo
    {
        public int cursorNr;
        public int playerNr;
        public LinkInfo(int _cursorNr, int _playerNr)
        {
            cursorNr = _cursorNr;
            playerNr = _playerNr;
        }
    }
}
