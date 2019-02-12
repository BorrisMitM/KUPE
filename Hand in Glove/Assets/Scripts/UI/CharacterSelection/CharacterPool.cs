using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CharacterPool : MonoBehaviour {
    public GameObject[] availableCharacters;
    public GameObject choser;
    public int panelAmount;
    public float yOffset;
    [HideInInspector]
    public List<GameObject>[] assignArray;
    private List<GameObject> firstList;
    private List<GameObject> secList;
    private List<GameObject> thrdList;
    private List<GameObject> fourthList;
    public static int lowBorder = 0;
    public static int topBorder = 3;
    private void Start()
    {
        InitLists();
        PlayerAmount();
        for (int i = lowBorder; i <= topBorder; i++)
        {
            GameObject obj = Instantiate(choser, availableCharacters[i].transform.position + new Vector3(0f, yOffset, 0f), Quaternion.identity, transform);
            assignArray[i - lowBorder].Add(obj);
        }
        for (int i = 0; i < availableCharacters.Length; i++)
            if (i < lowBorder || i > topBorder) Destroy(availableCharacters[i]);
    }

    private void PlayerAmount()
    {
        if (GameManager.playerAmount == 1)
            lowBorder = topBorder = 0;
        else if(GameManager.playerAmount == 2)
        {
            topBorder = 3;
            lowBorder = 2;
        }
        else if(GameManager.playerAmount == 3)
        {
            lowBorder = 0;
            topBorder = 2;
        }
        else
        {
            lowBorder = 0;
            topBorder = 3;
        }
    }

    public void RemoveList(int element)
    {
        if(assignArray[element].Count > 0)
        { 
            for (int i = assignArray[element].Count - 1; i >= 0; i--)  
            {
                assignArray[element][i].GetComponent<PlayerChoser>().GoRight();
            }
        }
        assignArray[element] = null;
    }

    private void InitLists()
    {
        firstList = new List<GameObject>();
        secList = new List<GameObject>();
        thrdList = new List<GameObject>();
        fourthList = new List<GameObject>();

        assignArray = new List<GameObject>[GameManager.playerAmount];

        if (GameManager.playerAmount > 0) assignArray[0] = firstList;
        if (GameManager.playerAmount > 1) assignArray[1] = secList;
        if (GameManager.playerAmount > 2) assignArray[2] = thrdList;
        if (GameManager.playerAmount > 3) assignArray[3] = fourthList;
    }
}
