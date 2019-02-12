using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetUseKeyboardPlayerPrefs : MonoBehaviour {
    private void OnEnable()
    {
        GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("UseKeyboard") == 1 ? true : false;
    }
    public void Set()
    {
        PlayerPrefs.SetInt("UseKeyboard", GetComponent<Toggle>().isOn ? 1 : 0);
        GameManager.useKeyboard = GetComponent<Toggle>().isOn;
    }
}
