using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : SwitchAction {

    public override void Do()
    {
        Destroy(gameObject);
    }
}
