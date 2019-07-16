﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground : MonoBehaviour
{
    public string groundTag;
    public bool grounded;
    public bool superGrounded;
    public LayerMask lm;
    public float rayLength;
    public float rayLengthOnGround;
    public float supersuperGroundedLength;
    public Collider2D[] deactivate;
    public Collider2D[] activate;
    [HideInInspector]
    public Vector2 normal;
    [HideInInspector]
    public Vector2 point;
    public float gravityAngle = 20f;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.up,grounded? rayLengthOnGround:rayLength,lm);
        bool g = hit.collider != null && hit.collider.tag == groundTag;
        if(g&&transform.position.magnitude < 10900){
            float gAn = Vector2.Angle(transform.position,hit.normal);
            if(gAn>gravityAngle){
                g= false;
            }
        }
        
        normal = hit.normal;
        point = hit.point;
        if(grounded != g){
            grounded = g;
            if(grounded){
                superGroundedCkeck = rayLength;
                foreach(Collider2D c in deactivate){
                    c.enabled = false;
                }
                foreach(Collider2D c in activate){
                    c.enabled = true;
                }
            }
            else{
                foreach(Collider2D c in deactivate){
                    c.enabled = true;
                }
                foreach(Collider2D c in activate){
                    c.enabled = false;
                }
            }
        }
        if(grounded){
            superGrounded = hit.distance<= superGroundedCkeck;
            if(superGrounded){
                superGroundedCkeck = rayLength;
                foreach(Collider2D c in deactivate){
                    c.enabled = false;
                }
                foreach(Collider2D c in activate){
                    c.enabled = true;
                }
            }
            else{
                superGroundedCkeck = supersuperGroundedLength;
                foreach(Collider2D c in deactivate){
                    c.enabled = true;
                }
                foreach(Collider2D c in activate){
                    c.enabled = false;
                }
            }
            //rb.angularVelocity = 0;
            //rb.rotation  = Mathf.Atan2(hit.normal.x,hit.normal.y)*Mathf.Rad2Deg;
            
        }
    }
    float superGroundedCkeck;
}