using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//closes submenu and activates main manu again

public class CloseSubMenu : MonoBehaviour {
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject controlsButton;
    void Update()
    {
        for (int i = 1; i <= 6; i++)
        {
            if (Input.GetButtonDown("Fire2_P" + i))
            {
                Button[] buttons = menuPanel.GetComponentsInChildren<Button>();
                foreach(Button b in buttons)
                {
                    b.interactable = true;
                }
                StartCoroutine(SetSelected());
            }
        }
    }
    //workaround to set selected gameobject{
    IEnumerator SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(controlsButton);
        gameObject.SetActive(false);
    }
}
