using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fade : MonoBehaviour {
    public FadeInfo[] fadeInfos;
    Image img;
	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        foreach (FadeInfo fi in fadeInfos)
            StartCoroutine(Fading(fi.time, fi.delay, fi.start, fi.end));
	}

    IEnumerator Fading(float time, float delay, float start, float end)
    {
        yield return new WaitForSeconds(delay);
        float fadeStop = Time.time + time;
        float startTime = Time.time;
        while(Time.time <= fadeStop)
        {
            img.color = new Color(1f, 1f, 1f, Mathf.Lerp(start, end, (Time.time - startTime) / (fadeStop - startTime)));
            yield return null;
        }
    }
    [System.Serializable]
    public struct FadeInfo
    {
        public float time, delay, start, end;
    }
}
