using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SubscribeToEventSystem : MonoBehaviour {

    private void OnEnable()
    {
        StartCoroutine(SelectContinueButtonLater());
    }

    IEnumerator SelectContinueButtonLater()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

}
