using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeButton : MonoBehaviour {

    public void Set()
    {
        SoundManager.SetMasterVolume(GetComponent<Slider>().value);
    }
}
