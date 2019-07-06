using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnToPos : MonoBehaviour
{
    Rigidbody2D rb;
    public float force;
    Rigidbody2D parent;
    Vector2 defaultPos;
    public float damp = 0.98f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPos = transform.localPosition;
        parent = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direct = transform.parent.TransformPoint( defaultPos)-transform.position;
        rb.AddForce(direct*force);
        Vector2 pointVel = parent.GetPointVelocity(parent.transform.TransformPoint(defaultPos));
        Vector2 extraVel = rb.velocity-pointVel;
        extraVel *= damp;
        rb.velocity = pointVel + extraVel;

    }
}
