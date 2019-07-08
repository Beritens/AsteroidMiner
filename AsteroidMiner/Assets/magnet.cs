﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    public float strength;
    public float radius;
    public LayerMask layerMask;

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,radius,layerMask);
        foreach(Collider2D col in colliders){
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if(rb != null){
                rb.AddForce(((Vector2)transform.position-rb.position).normalized*strength);
            }
        }
    }
}