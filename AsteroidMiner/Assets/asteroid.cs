using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid : MonoBehaviour
{
    public float health;
    public void damage(float amount){
        health-= amount;
        if(health <= 0){
            destroy();
        }
    }
    void destroy(){
        AsteroidBelt ab = GameObject.FindObjectOfType<AsteroidBelt>().GetComponent<AsteroidBelt>();
        ab.DeleteObject(transform);
        AsteroidBelt.Object drop = new AsteroidBelt.Object(transform.position-transform.parent.position,0,0,1,false);
        ab.AddObject(drop,transform.parent.gameObject,true);

        Destroy(gameObject);
    }
}
