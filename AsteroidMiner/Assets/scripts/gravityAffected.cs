using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityAffected : MonoBehaviour
{
    Rigidbody2D rb;
    float gravity = 9.81f;
    float start=10000;
    float end=10900;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(transform.position.magnitude >= end)
            return;
        float g= gravity*usefullStuff.map(((Vector2)transform.position).magnitude,start,end,1,0);
        rb.velocity += -((Vector2)transform.position.normalized*g)*Time.deltaTime;
    }
    
}
