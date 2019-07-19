using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour
{
    public static items instance;
    public item[] itemObjects;
    public GameObject defaultItem;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
    }
    public int GetIndex(item it){
        for(int i = 0; i<itemObjects.Length; i++){
            if(itemObjects[i] == it){
                return i;
            }
        }
        return -1;
    }
    public Transform DropItem(int item, Vector2 position){
        GameObject it  = GameObject.Instantiate(defaultItem,position,Quaternion.identity);
        it.GetComponent<SpriteRenderer>().sprite =  itemObjects[item].sprite;
        it.GetComponent<ressource>().item = item;
        return it.transform;
    }
}
