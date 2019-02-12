using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//activates the controls sub menu and disables and fades out the main panel buttons
public class ActivateControlsSubMenu : MonoBehaviour {

    [SerializeField]
    private GameObject subMenu;
    [SerializeField]
    private GameObject actionButton;
    public void Activate()
    {
        Button[] buttons = transform.parent.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            b.interactable = false;
        }
        subMenu.SetActive(true);
        StartCoroutine(SetSelected());
    }
    //workaround to set selected gameobject{
    IEnumerator SetSelected()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        yield return null;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(actionButton);
    }

}
