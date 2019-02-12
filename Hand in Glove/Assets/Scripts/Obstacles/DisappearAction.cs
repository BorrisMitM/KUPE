using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAction : SwitchAction
{

    public override void Do()
    {
        base.Do();
        gameObject.SetActive(false);
    }
}
