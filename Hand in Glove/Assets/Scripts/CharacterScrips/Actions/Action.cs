using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour {
    //abstract class for actions, than can be set to a key in the input manager
    public virtual void DoActionDown() { }
    public virtual void DoActionStay() { }
    public virtual void DoActionUp() { }
}
