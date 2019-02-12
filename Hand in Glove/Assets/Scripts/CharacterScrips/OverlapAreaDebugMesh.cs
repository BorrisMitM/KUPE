using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapAreaDebugMesh : MonoBehaviour {
    //little debug helper for Physics2D.OverlapArea
    CharacterInfo charInfo;
    Vector3 a, b; 
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update () {
        //charInfo = GetComponentInParent<CharacterInfo>();
        //a = (Vector2)transform.parent.position + new Vector2(0, charInfo.checkBosOffset) + new Vector2(charInfo.checkBoxXSize, charInfo.checkBoxYSize);
        //b = (Vector2)transform.parent.position + new Vector2(0, charInfo.checkBosOffset) + new Vector2(-charInfo.checkBoxXSize, -charInfo.checkBoxYSize);
        //Mesh overlapAreaMesh = new Mesh();
        //overlapAreaMesh.vertices = new Vector3[] { a, new Vector3(a.x, b.y, 0), new Vector3(b.x, a.y, 0), b };
        //overlapAreaMesh.triangles = new int[] { 0, 1, 2, 3, 2, 1 };
        //GetComponent<MeshFilter>().mesh = overlapAreaMesh;

    }
}
