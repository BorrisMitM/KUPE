using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Collectable : MonoBehaviour {
    [HideInInspector]
    public bool active = false;
    public EndDoor end;
    [SerializeField]
    private float rotationSpeed = 20f;

    private void Update()
    {
        if (active)
        {
            transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(active && collision.GetComponent<InputManager>())
        {
            end.CollectableCollected(this);
            Destroy(gameObject);
        }
    }
}
