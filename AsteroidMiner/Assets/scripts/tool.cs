using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tool : MonoBehaviour
{
    protected toolManager toolManager;
    item item;
    protected ground ground;
    public void instantiate(toolManager tM, int i){
        toolManager = tM;
        item = items.instance.itemObjects[i];
        ground = toolManager.GetComponent<ground>();
    }
    public virtual void OnSwitchSide(bool rightori){

    }
    public void lockIk(int i, bool yes){
        ground.lockIk(i,yes);
    }
}
