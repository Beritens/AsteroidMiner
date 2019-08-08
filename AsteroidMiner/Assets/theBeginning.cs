using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theBeginning : MonoBehaviour
{
    public Transform startRocketPosition;
    public int rocketItem;
    public int startItem;
    public void start(){
        inventory.instance.AddToTools(startItem,0);
        items.instance.spawnObject(rocketItem,startRocketPosition.position);
    }
}
