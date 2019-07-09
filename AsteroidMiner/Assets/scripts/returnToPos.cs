using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnToPos : MonoBehaviour
{
    Rigidbody2D rb;
    public float force;
    Rigidbody2D parent;
    Vector2 defaultPos;
    public Transform defaultPosObj;
    public float damp = 0.98f;
    public float maxDistance = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPos = defaultPosObj.localPosition;
        parent = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direct = transform.parent.TransformPoint( defaultPos)-transform.position;
        rb.AddForce(direct*force);
        if(direct.magnitude > maxDistance){
            transform.position = (Vector2)transform.parent.TransformPoint( defaultPos)-direct.normalized*maxDistance;
        }
        Vector2 pointVel = parent.GetPointVelocity(parent.transform.TransformPoint(defaultPos));
        Vector2 extraVel = rb.velocity-pointVel;
        extraVel *= damp;
        rb.velocity = pointVel + extraVel;

    }
}
