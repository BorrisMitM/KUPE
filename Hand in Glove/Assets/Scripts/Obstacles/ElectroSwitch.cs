using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ElectroSwitch : Switch
{
    protected override void Start()
    {
        base.Start();
        isTriggerArea = false;
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Eddi"))
            base.OnTriggerStay2D(collision);
    }
}
#if UNITY_EDITOR
    [CustomEditor(typeof(Switch))]
public class ElectroSwitchEditor : Editor
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