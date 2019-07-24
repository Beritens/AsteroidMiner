using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnItem : MonoBehaviour
{
    public int item;
    [ContextMenu("spawn")]
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        spawn();
    }
    public void spawn(){
        items.instance.DropItem(item,transform.position);
    }
}
