using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Switch : MonoBehaviour {
    [SerializeField]
    private bool reusable = false;
    [SerializeField]
    protected bool isTriggerArea = false;
    [SerializeField]
    protected SwitchAction[] actions;
    protected bool triggered = false;
    protected Switch[] switches;
    [SerializeField]
    private float pressedDuration = .5f;
    [SerializeField]
    private Sprite unpressed;
    [SerializeField]
    private Sprite pressed;
    [SerializeField]
    private Sprite activated;
    [SerializeField]
    private float cameraFocusDuration = 0.5f;
    private SpriteRenderer spriteRenderer;
    private CameraBehaviour cameraBehaviour;
    protected virtual void Start()
    {
        switches = FindObjectsOfType<Switch>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraBehaviour = FindObjectOfType<CameraBehaviour>();
        triggered = false;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (isTriggerArea)
            {
                Trigger();
            }
        }
        else if (collision.CompareTag("PlayerTrigger"))
        {
            if (isTriggerArea)
            {
                Trigger();
            }
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isTriggerArea && Input.GetButton("Fire2_P" + collision.GetComponent<InputManager>().inputNr.ToString()))
            {
                Trigger();
            }
        }
        else if (collision.CompareTag("PlayerTrigger"))
        {
            if (!isTriggerArea && Input.GetButton("Fire2_P" + collision.GetComponentInParent<InputManager>().inputNr.ToString()))
            {
                Trigger();
            }
        }
    }
    public void Trigger()
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine(SwitchSprites());
            if(GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();
            cameraBehaviour.SetSmoothSpeed(.8f);
            foreach (SwitchAction a in actions)
            {
                cameraBehaviour.AddObjToFollow(a.transform);
            }
            StartCoroutine(CameraOnSpawn());
        }
    }
    IEnumerator CameraOnSpawn()
    {
        yield return new WaitForSeconds(1f);
        foreach (SwitchAction a in actions)
        {
            a.Do();
        }
        foreach (Switch s in switches)
        {
            s.Reset();
        }
        yield return new WaitForSeconds(cameraFocusDuration);
        foreach (SwitchAction a in actions)
        {
            cameraBehaviour.RemoveObjToFollow(a.transform);
        }
        cameraBehaviour.ResetSmoothSpeed();
    }
    public void Reset()
    {
        if (reusable)
        {
            StopCoroutine(SwitchSprites());
            spriteRenderer.sprite = unpressed;
            triggered = false;
        }
    }

    protected IEnumerator SwitchSprites()
    {
        spriteRenderer.sprite = pressed;
        yield return new WaitForSeconds(pressedDuration);
        spriteRenderer.sprite = activated;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Switch))]
public class SwitchEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Switch myScript = (Switch)target;
        if (GUILayout.Button("Trigger"))
        {
            myScript.Trigger();
        }
    }
}
#endif
