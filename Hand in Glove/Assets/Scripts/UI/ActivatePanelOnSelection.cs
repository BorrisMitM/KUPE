using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ActivatePanelOnSelection : MonoBehaviour, ISelectHandler {

    [SerializeField]
    private GameObject[] panel;
    public GameObject[] otherPanels;
    public void OnSelect(BaseEventData eventData)
    {
        foreach (GameObject go in otherPanels)
            go.SetActive(false);
        foreach (GameObject go in panel)
            go.SetActive(true);
    }

}
