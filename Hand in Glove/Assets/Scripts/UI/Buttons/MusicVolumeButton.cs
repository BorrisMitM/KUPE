using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeButton : MonoBehaviour {

    public void Set()
    {
        SoundManager.SetMusicVolume(GetComponent<Slider>().value);
    }
}
