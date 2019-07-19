using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop : window
{
    public int[] products;
    public Transform productPanel;
    public GameObject prefab;
    inventory inv;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        inv = inventory.instance;
    }
    public override void pressE(){
        open();
        loadProducts();
    }
    void loadProducts(){
        foreach(Transform child in productPanel){
            Destroy(child.gameObject);
        }
        foreach(int i in products){
            GameObject g = GameObject.Instantiate(prefab,Vector2.zero,Quaternion.identity,productPanel);
            g.GetComponent<shopProduct>().setVariables(items.instance.itemObjects[i],true);
        }
    }
    public void productStuff(item i,bool buy){
        if(buy){
            
            inv.AddToSlots(items.instance.GetIndex(i));
        }
    }
}
