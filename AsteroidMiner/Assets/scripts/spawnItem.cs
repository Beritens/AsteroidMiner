using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnItem : MonoBehaviour
{
    public int item;
    AsteroidBelt ab;
    [ContextMenu("spawn")]
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        ab = GameObject.FindObjectOfType<AsteroidBelt>().GetComponent<AsteroidBelt>();
        spawn();
    }
    public void spawn(){
        ab.AddItem(item,transform.position,0,1);
        //items.instance.DropItem(item,transform.position);
    }
}
