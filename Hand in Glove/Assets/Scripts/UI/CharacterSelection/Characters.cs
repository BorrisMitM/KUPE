using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters")]
public class Characters : ScriptableObject {
    [SerializeField]
    public GameObject[] charPrefabs;
}
