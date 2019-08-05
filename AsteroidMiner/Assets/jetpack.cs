using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpack : active
{
    public float force;
    public GameObject fire;
    void FixedUpdate()
    {
        if(Input.GetButton("Jump")){
            player.AddForce(transform.up*force);
            fire.SetActive(true);
        } 
        else{
            fire.SetActive(false);
        }  
        
        
    }
}
