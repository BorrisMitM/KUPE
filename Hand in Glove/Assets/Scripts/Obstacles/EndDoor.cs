using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class EndDoor : MonoBehaviour {
    [SerializeField]
    public List<Collectable> collectables;
    public bool ready = false;
    public bool active = false;
    private static int playerCount;
    private List<string> playerNamesInTrigger;
    private EndDoor[] endDoors;
    [SerializeField]
    private Sprite activeSprite;
    [SerializeField]
    private Sprite doorSprite;
    // Use this for initialization
    void Start () {
        playerNamesInTrigger = new List<string>();
        foreach(Collectable c in collectables)
        {
            c.active = true;
            c.end = this;
        }
        playerCount = 0;
        endDoors = FindObjectsOfType<EndDoor>();
        if (collectables.Count == 0) ready = true;
	}
	
	public void CollectableCollected(Collectable collectable)
    {
        if (collectables.Contains(collectable))
            collectables.Remove(collectable);
        if (collectables.Count <= 0)
        {
            ready = true;
            foreach(EndDoor e in endDoors)
            {
                if (!e.ready) return;
            }
            foreach (EndDoor e in endDoors)
                e.Activate();
        }
    }
    public void Activate()
    {
        transform.Find("door").gameObject.GetComponent<SpriteRenderer>().sprite = doorSprite;
        GetComponent<SpriteRenderer>().sprite = activeSprite;
        active = true;
        if(playerCount >= GameManager.inputInformation.Count)
            EndScene();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playerNamesInTrigger.Contains(collision.gameObject.name))
                playerCount++;
            playerNamesInTrigger.Add(collision.gameObject.name);
            if (playerCount != GameManager.inputInformation.Count || !active) return;
            EndScene();
        }
    }
    private void EndScene()
    {
        Timer timer = FindObjectOfType<Timer>();
        if (timer)
        {
            timer.active = false;
        }
        LevelTimes.SaveTime(timer.time);
        GameManager.SaveLevel();
        string cutscene = GameManager.cutscenes.GetCutSceneNameAfterLevel(SceneManager.GetActiveScene().name);
        if(cutscene != null && !GameManager.doingRun)
        {
            SceneManager.LoadScene(cutscene);
        }
        else if (GameManager.doingRun && SceneManager.GetActiveScene().name.Contains("Virus"))
        {
            SceneManager.LoadScene("RUN");
        }
        else
        {
            SceneManager.LoadScene("BetweenLevelSelection");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNamesInTrigger.Remove(collision.gameObject.name);
            if (!playerNamesInTrigger.Contains(collision.gameObject.name))
                playerCount--;
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(EndDoor))]
public class EndDoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EndDoor myScript = (EndDoor)target;
        if (GUILayout.Button("Initialize"))
        {
            myScript.Activate();
        }
    }
}
#endif