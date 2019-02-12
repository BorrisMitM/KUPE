using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerSounds : MonoBehaviour {
    [SerializeField]
    private List<AudioClip> jumpClips;
    [SerializeField]
    private List<AudioClip> deathClips;
    [SerializeField]
    private List<AudioClip> oneLinerClips;
    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	public void Jump()
    {
        audioSource.PlayOneShot(jumpClips[Random.Range(0, jumpClips.Count)]);
    }

    public void Death()
    {
        audioSource.PlayOneShot(deathClips[Random.Range(0, deathClips.Count)]);
    }

    public void OneLiner()
    {
        audioSource.PlayOneShot(oneLinerClips[Random.Range(0, oneLinerClips.Count)]);
    }
}
