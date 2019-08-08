using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadPlayer : MonoBehaviour
{
    public Transform[] limbs;
    public void reload(SaveDataPlayer sDP, List<Transform> objects){
        
        Rigidbody2D player = GetComponent<Rigidbody2D>(); 
        Vector2 pos = new Vector2(sDP.position[0],sDP.position[1]);
        Vector2 vel = new Vector2(sDP.velocity[0],sDP.velocity[1]);
        transform.position = pos;
        transform.eulerAngles = new Vector3(0,0,sDP.rotaion);
        player.velocity = vel;
        Camera.main.transform.parent.position = pos;
        foreach(Transform t in limbs){
            returnToPos rTP= t.GetComponent<returnToPos>();
            Rigidbody2D rb = t.GetComponent<Rigidbody2D>();
            //Debug.Log(vel + ""+ player.velocity+ player.GetPointVelocity(player.position));
            
            
            t.localPosition = rTP.defaultPosObj.localPosition;
            t.rotation = Quaternion.identity;
            rb.velocity = player.velocity;
            //Debug.Log(rb.velocity + " "+ player.velocity);
            //rb.velocity = player.GetPointVelocity(rTP.defaultPosObj.position);
        }
        if(sDP.parent >= 0){
            IparentForPlayer par = objects[sDP.parent].GetComponent<IparentForPlayer>();
            if(par != null)
            {
                par.activateIt();
            }
        }
    }
}
