using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRopeAnimation : MonoBehaviour {
    private Animator withArmKaren;
    private Animator withoutArmKaren;
    // Use this for initialization
    void Start () {
        withArmKaren = transform.Find("c_karen").gameObject.GetComponent<Animator>();
        withoutArmKaren = transform.Find("c_karen_rope").gameObject.GetComponent<Animator>();
	}
	
	public void ChangeActiveModel(bool roping)
    {
        if (roping)
        {
            withoutArmKaren.gameObject.SetActive(true);
            withArmKaren.gameObject.SetActive(false);
            GetComponent<CharacterInfo>().animator = withoutArmKaren;
            GetComponent<Movement>().animator = withoutArmKaren;
            GetComponent<Jump>().animator = withoutArmKaren;
            GetComponent<Death>().animator = withoutArmKaren;
            GetComponent<RopeAction>().animator = withoutArmKaren;
        }
        else
        {
            withoutArmKaren.gameObject.SetActive(false);
            withArmKaren.gameObject.SetActive(true);
            GetComponent<CharacterInfo>().animator = withArmKaren;
            GetComponent<Movement>().animator = withArmKaren;
            GetComponent<Jump>().animator = withArmKaren;
            GetComponent<Death>().animator = withArmKaren;
            GetComponent<RopeAction>().animator = withArmKaren;
        }
    }
}
