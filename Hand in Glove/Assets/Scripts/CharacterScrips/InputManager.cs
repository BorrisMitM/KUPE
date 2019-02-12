using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[RequireComponent(typeof(Movement))]
public class InputManager : MonoBehaviour {
    [Header("Player")]
    public int inputNr;
    public string horizontalString;
    private string fire1String;
    private string jumpString;
    private string fire2String;
    
    [Header("Actions")]
    [SerializeField]
    private Action fire1Action; // A button
    [SerializeField]
    private Action jumpAction; // B button
    [SerializeField]
    private Action fire2Action; // X button
    private Movement movement;
    
    private bool inititalized = false;
    private CharacterInfo charInfo;
    [HideInInspector]
    public bool active = true;
    private bool paused = false;
    private bool jump;
    private bool fire1;
    private bool fire2;

    private void Start()
    {
        movement = GetComponent<Movement>();
        charInfo = GetComponent<CharacterInfo>();
        active = true;
    }

    public void InitializeInput(int _inputNr)
    {
        inputNr = _inputNr;
        horizontalString = "Horizontal_P" + inputNr;
        fire1String = "Fire1_P" + inputNr;
        jumpString = "Fire2_P" + inputNr;
        fire2String = "Jump_P" + inputNr;
        inititalized = true;
    }

    public void Deactivate()
    {
        active = false;
        if (Input.GetButton(jumpString) && jumpAction) jumpAction.DoActionUp();
        if (Input.GetButton(fire1String) && fire1Action) fire1Action.DoActionUp();
        if (Input.GetButton(fire2String) && fire2Action) fire2Action.DoActionUp();
    }
    void Update () {
        //handle Inputs
        if (charInfo.canMove && (active & inititalized & !GameManager.paused))
        {
            movement.Move(Input.GetAxisRaw(horizontalString));
        }
        else movement.Move(0f);
        if(GameManager.paused && !paused) //prevent weird behaviours when pausing
        {
            jump = Input.GetButton(jumpString);
            fire1 = Input.GetButton(fire1String);
            fire2 = Input.GetButton(fire2String);
            paused = true;
        }
        else if(!GameManager.paused && paused)
        {
            if (jump != Input.GetButton(jumpString)) if (jump) jumpAction.DoActionUp(); else jumpAction.DoActionDown(); 
            if (fire1 != Input.GetButton(fire1String)) if (fire1) fire1Action.DoActionUp(); else fire1Action.DoActionDown(); 
            if (fire2 != Input.GetButton(fire2String)) if (fire2) fire2Action.DoActionUp(); else fire2Action.DoActionDown();
            paused = false;
        }
        if (!active || !inititalized || GameManager.paused) return;
        if (Input.GetButtonDown(jumpString) && jumpAction) jumpAction.DoActionDown();
        if (Input.GetButton(jumpString) && jumpAction) jumpAction.DoActionStay();
        if (Input.GetButtonUp(jumpString) && jumpAction) jumpAction.DoActionUp();
        if (Input.GetButtonDown(fire1String) && fire1Action) fire1Action.DoActionDown();
        if (Input.GetButton(fire1String) && fire1Action) fire1Action.DoActionStay();
        if (Input.GetButtonUp(fire1String) && fire1Action) fire1Action.DoActionUp();
        if (Input.GetButtonDown(fire2String) && fire2Action) { fire2Action.DoActionDown();}
        if (Input.GetButton(fire2String) && fire2Action) fire2Action.DoActionStay();
        if (Input.GetButtonUp(fire2String) && fire2Action) fire2Action.DoActionUp();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    int i = 5;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InputManager myScript = (InputManager)target;
        i = EditorGUILayout.IntSlider("InputNr", i, 1, 6);
        if (GUILayout.Button("Initialize"))
        {
            myScript.InitializeInput(i);
        }
    }
}
#endif