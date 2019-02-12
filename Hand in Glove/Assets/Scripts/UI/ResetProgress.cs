using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgress : MonoBehaviour {

	public void Do()
    {
        PlayerPrefs.DeleteAll();
    }
}
