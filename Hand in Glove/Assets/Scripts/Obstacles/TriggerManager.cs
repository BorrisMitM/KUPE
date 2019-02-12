using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TriggerManager : SwitchAction
{
    [SerializeField]
    private SwitchAction[] firstActions;
    [SerializeField]
    private SwitchAction[] secondActions;
    private int count = 0;

    public override void Do()
    {
        base.Do();
        if(count == 0)
        {
            foreach (SwitchAction a in firstActions)
            {
                a.Do();
            }
            count++;
        }
        else
        {
            foreach (SwitchAction a in secondActions)
            {
                a.Do();
            }
            count = 0;
        }
    }
}

