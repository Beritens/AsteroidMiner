using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectObj : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        Vector2 camPos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetButtonDown("interact")){
           Transform sel = mouseOver();
           if(sel == null)
                return;
           //if(Vector2.Distance(camPos,sel.position)<3f){
               if(sel.GetComponent<selectable>()!= null){
                   sel.GetComponent<selectable>().pressE();
               }
           //}
        }
    }
    public static Transform mouseOver(){
        Vector2 camPos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(camPos,Vector3.zero);
        Transform trans= null;
        foreach(RaycastHit2D hit in hits){
            if(trans==  null)
                trans = hit.transform;
            else{
                if(Vector2.Distance(camPos,hit.transform.position)<Vector2.Distance(camPos,trans.position)){
                    trans = hit.transform;
                }
            }
        }
        return trans;
    }   
}
