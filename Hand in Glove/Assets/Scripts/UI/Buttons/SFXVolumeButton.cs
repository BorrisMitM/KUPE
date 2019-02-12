using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SFXVolumeButton : MonoBehaviour {

	public void Set()
    {
        SoundManager.SetSFXVolume(GetComponent<Slider>().value);
    }
}
