using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParticleColor : MonoBehaviour {
    ParticleSystem particle;
    public PlayerType playerType;
	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();
        ChangeColor();
    }
	
	public void ChangeColor(Color _color)
    {
        ParticleSystem.MainModule main = particle.main;
        main.startColor = _color;
    }

    public void ChangeColor()
    {
        ParticleSystem.MainModule main = particle.main;
        Color color = new Color(1f,1f,1f,0f);
        if (playerType == PlayerType.BouncyGuy) color = Color.red;
        else if(playerType == PlayerType.FlyGuy) color = Color.green;
        else if (playerType == PlayerType.ElectroGirl) color = Color.yellow;
        else if (playerType == PlayerType.RopeGirl) color = Color.blue;
        main.startColor = color;
    }
}
