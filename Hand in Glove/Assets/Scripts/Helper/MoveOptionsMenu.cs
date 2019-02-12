using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveOptionsMenu : MonoBehaviour {

    [SerializeField]
    private float xVisible = -310f;

    [SerializeField]
    private float xNonVisible = 400f;

    [SerializeField]
    private GameObject buttonAfterFadeOut;
    private float moveStartTime;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    public void FadeMenuIn(float duration)
    {
        StartCoroutine(MoveMenu(xVisible, duration, true));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StopCoroutine("MoveMenu");
            FadeMenuOut(0.3f);
        }
    }
    public void FadeMenuOut(float duration)
    {
        StartCoroutine(MoveMenu(xNonVisible, duration, false));
        StartCoroutine(SelectContinueButtonLater());
    }

    private IEnumerator MoveMenu(float xDest, float duration, bool fadeIn)
    {
        //if (fadeIn) gameObject.SetActive(true);
        moveStartTime = Time.time;
        float xStart = rectTransform.anchoredPosition.x;
        while(moveStartTime + duration >= Time.time)
        {
            float t = (Time.time - moveStartTime) / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);     //ease out
            rectTransform.anchoredPosition = new Vector3(Mathf.Lerp(xStart, xDest, t) , transform.localPosition.y); 
            yield return null;
        }
        if (!fadeIn) gameObject.SetActive(false);
    }
    IEnumerator SelectContinueButtonLater()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonAfterFadeOut);
    }
}
