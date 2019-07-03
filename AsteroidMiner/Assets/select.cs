using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class select : MonoBehaviour
{
    //SpriteRenderer spriteRenderer;
    Light2D  area;

    void Start()
    {
        area = GetComponent<Light2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void selectObject(GameObject obj, Color col = default(Color)){
        if(obj.GetComponent<Collider2D>() == null){
            return;
        }
        if(col == default(Color)){
            col = Color.white;
        }
        area.enabled = true;
        area.color = col;
        Bounds bounds = obj.GetComponent<Collider2D>().bounds;
        transform.position = bounds.center;
        transform.localScale = bounds.size;
    }
    public void deselect(){
        area.enabled = false;
    }
}
