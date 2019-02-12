using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeathChooser : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string text = "";
        int least = 10000000;
        List<PlayerType> leastPlayerType = new List<PlayerType>();
        List<PlayerType> bla = new List<PlayerType>();
        foreach(KeyValuePair<PlayerType, int> i in Death.playerDeaths)
        {
            if (least > i.Value)
            {
                least = i.Value;
                leastPlayerType = new List<PlayerType>();
                leastPlayerType.Add(i.Key);
            }
            else if(least == i.Value)
                leastPlayerType.Add(i.Key);

            bla.Add(i.Key);
        }
        foreach (PlayerType pt in leastPlayerType)
        {
            if (pt == (PlayerType.BouncyGuy) && bla.Contains(PlayerType.BouncyGuy))
            {
                if (leastPlayerType.IndexOf(PlayerType.BouncyGuy) > 0) text += ", ";
                text += "Ute";
            }
            if (pt == (PlayerType.RopeGirl) && bla.Contains(PlayerType.RopeGirl))
            {
                if (leastPlayerType.IndexOf(PlayerType.RopeGirl) > 0) text += ", ";
                text += "Karen";
            }
            if (pt == (PlayerType.FlyGuy) && bla.Contains(PlayerType.FlyGuy))
            {
                if (leastPlayerType.IndexOf(PlayerType.FlyGuy) > 0) text += ", ";
                text += "Peter";
            }
            if (pt == (PlayerType.ElectroGirl) && bla.Contains(PlayerType.ElectroGirl))
            {
                if (leastPlayerType.IndexOf(PlayerType.ElectroGirl) > 0) text += ", ";
                text += "Eddi";
            }
        }
        GetComponent<Text>().text = text;
    }
}
