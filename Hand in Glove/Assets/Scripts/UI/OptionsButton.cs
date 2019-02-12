using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour {

    [SerializeField]
    private GameObject optionsMenu;

    public void ActivateOptionsMenu()
    {
        optionsMenu.SetActive(true);
        optionsMenu.GetComponent<MoveOptionsMenu>().FadeMenuIn(0.3f);
    }
}
