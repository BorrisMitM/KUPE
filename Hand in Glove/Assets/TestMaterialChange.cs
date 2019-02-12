using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestMaterialChange : MonoBehaviour {
    [ColorUsage(false, true)]
    public Color _color;
    [ColorUsage(false, true)]
    public Color _color2;
    public Material testMaterial;

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time, 1f);
        Color myColor = Color.Lerp(_color, _color2, t);
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        testMaterial.SetColor("_EmissionColor", myColor);
    }

}
