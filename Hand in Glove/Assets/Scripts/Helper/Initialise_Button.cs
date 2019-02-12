using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Backup script that assures that there's always a button selected even if a mousclick deselects
public class Initialise_Button : MonoBehaviour
{
    GameObject lastselect;
    void Start()
    {
        lastselect = new GameObject();
    }
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current == null) return;
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }
}
