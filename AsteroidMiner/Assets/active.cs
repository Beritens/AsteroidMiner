using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class active : MonoBehaviour
{
    protected Rigidbody2D player;
    protected ActiveManager activeManager;
    public void instantiate(ActiveManager active){
        activeManager = active;
        player = active.GetComponent<Rigidbody2D>();
    }
}
