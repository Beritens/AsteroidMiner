using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dampRelativeVel : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D parentRb;
    Rigidbody2D rb;
    float angle;
    float radius;
    public float damp;
    void Start()
    {
        Transform parent = transform.parent;
        parentRb = parent.GetComponent<Rigidbody2D>();
        Vector2 relativePosition= parent.InverseTransformPoint(transform.position);
        Vector2 cOM = parentRb.centerOfMass;
        Vector2 relativeToCOM = relativePosition-cOM;
        radius = relativeToCOM.magnitude;
        angle = Vector2.SignedAngle(Vector2.right,relativeToCOM);
        Debug.Log(angle);
        rb = GetComponent<Rigidbody2D>();
        // Debug.Log(relativePosition.x + "|"+relativePosition.y);
        // Debug.Log(relativeToCOM.x + "|"+relativeToCOM.y);
        // Debug.Log(angle + " "+radius);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velNow = rb.velocity;
        Vector2 VelParent= parentRb.velocity;
        float speed = radius*parentRb.angularVelocity;
        float degree = parentRb.rotation+angle+90;
        Debug.DrawRay(parentRb.transform.TransformPoint(parentRb.centerOfMass),Vector2Extension.DegreeToVector2(parentRb.rotation+angle)*radius);
        Debug.Log(Vector2Extension.DegreeToVector2(degree));
        Vector2 normalVel = Vector2Extension.DegreeToVector2(degree) * speed + VelParent;     
        Vector2 relativeVel = velNow-normalVel;
        relativeVel*=  damp;
        Debug.DrawRay((Vector2)parentRb.transform.TransformPoint(parentRb.centerOfMass)+Vector2Extension.DegreeToVector2(parentRb.rotation+angle)*radius,Vector2Extension.DegreeToVector2(degree),Color.red);
        rb.velocity = normalVel +relativeVel;  
    }
}
