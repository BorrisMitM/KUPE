using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderValuePlayerPrefs : MonoBehaviour {
    [SerializeField]
    private string saveName;
    private void OnEnable()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(saveName, 1);
    }
}
