using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    public Transform container;
    items items;
    void Start()
    {
        items = items.instance;
    }

    public void AddActive(int item, int slot, int count){
        Transform slotT = container.GetChild(slot);
        if(slotT.childCount > 0){
            foreach (Transform child in slotT)
            {
                Destroy(child.gameObject);
            }
        }
        if(count == 0){
            return;
        }
        else{
            GameObject g = GameObject.Instantiate(items.itemObjects[item].prefab,transform.position,slotT.rotation,slotT);
            g.GetComponent<active>().instantiate(this);
        }
    }
}
