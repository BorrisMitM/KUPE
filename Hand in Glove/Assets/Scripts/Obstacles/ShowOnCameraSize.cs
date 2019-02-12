using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnCameraSize : MonoBehaviour {
    [SerializeField]
    private float showSize = 15f;
    private Camera cam;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(cam.orthographicSize < showSize)
        {
            col.enabled = true;
            spriteRenderer.enabled = true;
        }
        else
        {
            col.enabled = false;
            spriteRenderer.enabled = false;
        }
	}
}
