using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateEmission : MonoBehaviour {

    [ColorUsage(false, true)]
    public Color normalColor;
    [ColorUsage(false, true)]
    public Color lowPulsColor;
    [ColorUsage(false, true)]
    public Color highPulsColor;
    [SerializeField]
    private Material material;
    public bool pulse = false;
    private bool pulsed = false;
    // Use this for initialization
    void Start () {
        material.SetColor("_EmissionColor", normalColor);
    }
	
	// Update is called once per frame
	void Update () {
        if (pulse)
        {
            pulsed = true;
            float t = Mathf.PingPong(Time.time, 1f);
            Color lerpedColod = Color.Lerp(lowPulsColor, highPulsColor, t);
            material.SetColor("_EmissionColor", lerpedColod);

        }
        else if (pulsed)
        {
            pulsed = false;
            material.SetColor("_EmissionColor", normalColor);
        }
	}
}
