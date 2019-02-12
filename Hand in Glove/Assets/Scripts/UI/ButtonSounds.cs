using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, ISelectHandler {

    private AudioSource source;
    public AudioClip confirmClip;
    public AudioClip selectClip;
	// Use this for initialization
	void Start () {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = confirmClip;
        source.playOnAwake = false;
        AudioMixer mixer = Resources.Load("Master") as AudioMixer;
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        GetComponent<Button>().onClick.AddListener(() => PlaySound());
	}
	
	public void PlaySound()
    {
        source.PlayOneShot(confirmClip);
    }

    public void OnSelect(BaseEventData eventData)
    {
        source.PlayOneShot(selectClip);
    }
}
