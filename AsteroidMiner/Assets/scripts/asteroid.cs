﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid : MonoBehaviour
{
    public float health;
    float h;
    public Sprite[] stages;
    public float dropCircle = 1;
    public Vector2Int dropCountRange;
    SpriteRenderer r;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        h= health;
    }
    public void damage(float amount){
        if(r == null)
            r = GetComponent<SpriteRenderer>();
        
        h-= amount;
        if(h <= 0){
            destroy();
            return;
        }
        int index = Mathf.FloorToInt((stages.Length+1)-(h/health)*(stages.Length+1));
        if(index == 0){
            return;
        }
        r.sprite = stages[index-1];
        
        
    }
    void destroy(){
        AsteroidBelt ab = GameObject.FindObjectOfType<AsteroidBelt>().GetComponent<AsteroidBelt>();
        ab.DeleteObject(transform);
        int count = Random.Range(dropCountRange.x,dropCountRange.y);
        while( count > 0){
            count--;
            AsteroidBelt.Object drop = new AsteroidBelt.Object((transform.position+Random.insideUnitSphere*dropCircle)-transform.parent.position,0,0,1,false);
            ab.AddObject(drop,transform.parent.gameObject,true);
        }
        

        Destroy(gameObject);
    }
}