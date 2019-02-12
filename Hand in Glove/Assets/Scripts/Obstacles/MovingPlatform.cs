using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MovingPlatform : MonoBehaviour {
    [SerializeField]
    private Vector2[] locations;
    public float velocity;
    [SerializeField]
    private bool loopAround = true;
    [HideInInspector]
    public Vector2 dir;
    public bool active = true;
    private Rigidbody2D rb;
    private int currentLocationID;
    private float reachingLocationTime;
    private Vector2 startPoint;
    private Vector2 destPoint;
    private float startTime;
    private int coundDir;
    private bool destructable;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        transform.position = locations[0];
        coundDir = 1;
    }
    void Update () {
        if(active)
        {
             MoveTowardsLocationTransform();
        }
    }

    private void MoveTowardsLocationTransform()
    {
        rb.isKinematic = true;
        if (Time.time >= startTime + reachingLocationTime)
        {
            transform.position = locations[currentLocationID];
            startPoint = locations[currentLocationID];
            ChangeCurrentLocationId();
            destPoint = locations[currentLocationID];
            reachingLocationTime = (destPoint - startPoint).magnitude / velocity;
            startTime = Time.time;
        }
        else
            transform.position = Vector2.Lerp(startPoint, destPoint, (Time.time - startTime) / reachingLocationTime);
    }

    private void ChangeCurrentLocationId()
    {
        if (destructable)
        {
            currentLocationID++;
            if (currentLocationID >= locations.Length)
            {
                foreach(Transform c in transform)
                {
                    if (c.CompareTag("Player"))
                        c.parent = null;
                }
                currentLocationID = 0;  
                Destroy(gameObject);
            }

            return;
        }
        if (loopAround)
        {
            currentLocationID++;
            if (currentLocationID >= locations.Length) currentLocationID = 0;
        }
        else
        {
            if (currentLocationID >= locations.Length - 1) coundDir = -1;
            else if (currentLocationID <= 0) coundDir = 1;
            currentLocationID += coundDir;
        }
    }
    private void MoveTowardsLocation()
    {
        dir = (locations[currentLocationID] - (Vector2)transform.position).normalized;
        float distanceToLocation = (locations[currentLocationID] - (Vector2)transform.position).magnitude;
        if (Time.deltaTime * velocity > distanceToLocation)
        {
            rb.velocity = dir * distanceToLocation / Time.deltaTime;
            currentLocationID++;
            if (currentLocationID >= locations.Length) currentLocationID = 0;
        }
        else
            rb.velocity = dir * velocity;
    }
    public void Set(Vector2[] _locations, float _velocity, bool _destructable, bool _looparound)
    {
        locations = new Vector2[_locations.Length];
        for(int i = 0; i < _locations.Length; i++) 
        {
            locations[i] = _locations[i];
        }
        velocity = _velocity;
        destructable = _destructable;
        loopAround = _looparound;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 size = transform.localScale * GetComponent<BoxCollider2D>().size;
        int j = 1;
        foreach(Vector2 l in locations)
        {
            Gizmos.DrawWireCube(l, size);
            Handles.Label(l, j.ToString());
            j ++;
        }

        for (int i = 1; i < locations.Length; i++)
            Gizmos.DrawLine(locations[i - 1], locations[i]);
        if(loopAround) Gizmos.DrawLine(locations[locations.Length - 1], locations[0]);
    }
#endif
}

