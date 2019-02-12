using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyboardAmountSlider : MonoBehaviour {
    [SerializeField]
    private Text amountOfKeyboardPlayersText;
	// Use this for initialization
	void Start () {
        GetComponent<Slider>().value = GameManager.keyBoardPlayersAmount;
        amountOfKeyboardPlayersText.text = "Amount of Keyboard Players: " + GameManager.keyBoardPlayersAmount.ToString();
    }
	
	public void ValueChanged()
    {
        GameManager.keyBoardPlayersAmount = Mathf.RoundToInt(GetComponent<Slider>().value);
        PlayerPrefs.SetInt("KeyBoardPlayersAmount", GameManager.keyBoardPlayersAmount);
        amountOfKeyboardPlayersText.text = "Amount of Keyboard Players: " + Mathf.RoundToInt(GetComponent<Slider>().value).ToString();
    }
}
