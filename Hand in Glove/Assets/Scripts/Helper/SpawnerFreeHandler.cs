using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFreeHandler : MonoBehaviour {
    public bool free = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) free = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) free = true;
    }
}
