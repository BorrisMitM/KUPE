using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MovingPlatormSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject movingPlatformPrefab;
    [SerializeField]
    private Vector2[] locations;
    [SerializeField]
    private Vector2 size = new Vector2(2f, 0.5f);
    [SerializeField]
    private float velocity = 2.5f;
    [SerializeField]
    private bool destructable = true;
    [SerializeField]
    private bool loopAround = false;
    [SerializeField]
    private float timeBetweenSpawns = 2f;
    [SerializeField]
    private float spawnDelay = 0f;
    private float timeBetweenSpawnsCounter;
    // Use this for initialization
    void Start () {
        timeBetweenSpawnsCounter = timeBetweenSpawns - spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
        timeBetweenSpawnsCounter += Time.deltaTime;
		if(timeBetweenSpawnsCounter >= timeBetweenSpawns)
        {
            MovingPlatform movingPlatform = Instantiate(movingPlatformPrefab, locations[0], Quaternion.identity, transform).GetComponent<MovingPlatform>();
            movingPlatform.Set(locations, velocity, destructable, loopAround);
            movingPlatform.transform.localScale = size;
            movingPlatform.GetComponent<MovingPlatformAction>().enabled = false;
            timeBetweenSpawnsCounter -= timeBetweenSpawns;
        }
	}
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 mySize = new Vector3(size.x * 2f, size.y * .4f, 0f);
        int j = 1;
        foreach (Vector2 l in locations)
        {
            Gizmos.DrawWireCube(l, mySize);
            Handles.Label(l, j.ToString());
            j++;
        }

        for (int i = 1; i < locations.Length; i++)
            Gizmos.DrawLine(locations[i - 1], locations[i]);
        if (loopAround) Gizmos.DrawLine(locations[locations.Length - 1], locations[0]);
    }
#endif
}
