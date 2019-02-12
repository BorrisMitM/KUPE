using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Portal : MonoBehaviour {
    [SerializeField]
    private float spawnOffset = -1f;
    [SerializeField]
    private Portal otherPortal;
    private List<GameObject> justPortedGameObjects;
    private Animator anim;
    private void Start()
    {
        justPortedGameObjects = new List<GameObject>();
        anim = GetComponent<Animator>();
        Portal[] otherPortals = FindObjectsOfType<Portal>();
        bool fromOtherPortal = false;
        foreach(Portal p in otherPortals)
        {
            if (p.otherPortal == this) fromOtherPortal = true;
        }
        if (otherPortal == null) anim.SetTrigger("out");
        else if (otherPortal != null && !fromOtherPortal) anim.SetTrigger("in");
    }
    public void Teleport(GameObject ported)
    {
        ported.transform.position = transform.position + Vector3.up * spawnOffset;
        ported.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        justPortedGameObjects.Add(ported);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if(go != null && !justPortedGameObjects.Contains(go))
        {
            if(otherPortal != null)
            {
                otherPortal.Teleport(go);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go != null && justPortedGameObjects.Contains(go))
        {
            justPortedGameObjects.Remove(go);
        }
    }
}
