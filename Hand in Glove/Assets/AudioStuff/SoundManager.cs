using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
//manages the persistent background music and sound levels in the game
public class SoundManager : MonoBehaviour {
    public static float masterVolume;
    public static float musicVolume;
    public static float sfxVolume;
    [SerializeField]
    private static AudioMixer masterMixer;
    [SerializeField]
    private AudioClip menuClip;
    [SerializeField]
    private AudioClip playingClip;
	// Use this for initialization
	void Awake ()
    {
        masterMixer = (AudioMixer)Resources.Load("Master"); 
        Init();
	}
	
    void Init()     //Loads saved volumes and applies them
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume",1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume",1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume",1f);
        masterMixer.SetFloat("MasterVolume", ToDB(masterVolume));
        masterMixer.SetFloat("MusicVolume", ToDB(musicVolume));
        masterMixer.SetFloat("SFXVolume", ToDB(sfxVolume));
    }
    //setters for the different volumes
	public static void SetMasterVolume(float vol)
    {
        if (masterMixer == null) return;
        masterVolume = vol;
        PlayerPrefs.SetFloat("MasterVolume", vol);
        masterMixer.SetFloat("MasterVolume", ToDB(vol));
    }
    public static void SetMusicVolume(float vol)
    {
        if (masterMixer == null) return;
        musicVolume = vol;
        PlayerPrefs.SetFloat("MusicVolume", vol);
        masterMixer.SetFloat("MusicVolume", ToDB(vol));
    }
    public static void SetSFXVolume(float vol)
    {
        if (masterMixer == null) return;
        sfxVolume = vol;
        PlayerPrefs.SetFloat("SFXVolume", vol);
        masterMixer.SetFloat("SFXVolume", ToDB(vol));
    }
    //translates the percentage of volume to db not really right should be exponential but works okay
    private static float ToDB(float vol)
    {
        return Mathf.Lerp(-50f, 0f, vol);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //applies the right music for the corresponding scenes
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        masterMixer.SetFloat("MasterVolume", ToDB(masterVolume));
        masterMixer.SetFloat("MusicVolume", ToDB(musicVolume));
        masterMixer.SetFloat("SFXVolume", ToDB(sfxVolume));
        if (scene.name == "CharacterSelection" || scene.name == "1_MainMenu" || 
            scene.name == "Options" || scene.name == "BetweenLevelSelection"|| 
            scene.name == "LevelSelection" || scene.name == "ObstacleInformation" || 
            scene.name == "CharacterControls" || scene.name == "InteractionControls")
        {
            if(GetComponent<AudioSource>().clip != menuClip)
            {
                GetComponent<AudioSource>().clip = menuClip;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (scene.name.Contains("Cutscene"))
        {
            GetComponent<AudioSource>().Pause();
            GetComponent<AudioSource>().clip = null;
        }
        else if (GetComponent<AudioSource>().clip != playingClip)
        {
            GetComponent<AudioSource>().clip = playingClip;
            GetComponent<AudioSource>().Play();
        }
    }
}
