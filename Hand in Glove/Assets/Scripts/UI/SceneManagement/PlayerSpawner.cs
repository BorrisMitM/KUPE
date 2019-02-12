using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerSpawner : MonoBehaviour {
    private Transform[] spawnPos;
    [SerializeField]
    public bool spawnOnStart;
    // Use this for initialization
    void Start () {
        
        //if (spawnOnStart ) Spawn();
    }
    [ContextMenu("Spawn")]
    public void Spawn()
    {
        spawnPos = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            spawnPos[i] = child;
            i++;
        }
        i = 0;
        foreach (InputInformation inputInformation in GameManager.inputInformation)
        {
            GameObject player = Instantiate(inputInformation.characterPrefab, transform.Find(inputInformation.characterPrefab.name + "Spawn").position, Quaternion.identity);
            player.GetComponent<InputManager>().InitializeInput(inputInformation.inputNr);
            i++;
        }
    }
    public Vector2 GetFreeSpawnLocation(string deadPlayerName)
    {
        int found = deadPlayerName.IndexOf("(");
        deadPlayerName = deadPlayerName.Remove(found);
        return transform.Find(deadPlayerName + "Spawn").position;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerSpawner))]
public class PlayerSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerSpawner myScript = (PlayerSpawner)target;
        if (GUILayout.Button("Spawn"))
        {
            myScript.Spawn();
        }
    }
}
#endif