using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerOfMass : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 defaultCenter;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        changeCenter(defaultCenter);
    }

    public void changeCenter(Vector2 newCenter){
        rb.centerOfMass = newCenter;
    }

}
