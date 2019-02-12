using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AnimatorController : MonoBehaviour {
    bool justChecked;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelection" && !gameObject.name.Contains("karen"))
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            GetComponent<Animator>().SetBool("sel_idle", true);
        }
    }
    // Update is called once per frame
    void Update () {
        if (Time.time % 1f < 0.1f)
        {
            if (!justChecked)
            {
                if (Random.Range(0f, 1f) < .2f)
                    GetComponent<Animator>().SetBool("idle02", true);
                else GetComponent<Animator>().SetBool("idle02", false);
                justChecked = true;
            }
        }
        else justChecked = false;
	}

    public void SetChosen()
    {
        GetComponent<Animator>().SetTrigger("sel_chosen");
    }
}
