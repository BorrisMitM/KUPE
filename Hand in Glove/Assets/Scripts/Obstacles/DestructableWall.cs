using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour {
    [SerializeField]
    private float breakvelocity = 20f;
    [SerializeField]
    private bool withoutUte = false;
    [SerializeField]
    private int hitAmount = 3;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private GameObject particles;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[10];
        collision.GetContacts(contactPoints);
        //foreach (ContactPoint2D cp2d in contactPoints)
        //{
        //    if (Mathf.Abs(cp2d.normal.x) <= Mathf.Abs(cp2d.normal.y)) return;
        //}
        if (Mathf.Abs(contactPoints[0].normal.x) <= Mathf.Abs(contactPoints[0].normal.y)) return;
        if (!withoutUte)
        {
            if (collision.gameObject.GetComponent<BouncyGuyInteraction>() != null)
            {
                if (collision.relativeVelocity.magnitude >= breakvelocity)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;
                    GameObject par = Instantiate(particles, transform.position, Quaternion.identity);
                    Destroy(par, 1f);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                hitAmount--;
                GameObject par = Instantiate(particles, transform.position, Quaternion.identity);
                Destroy(par, 1f);
                Destroy(gameObject);
            }
        }
    }
    [ContextMenu("Do Something")]
    public void Bla()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
