using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform target;
    public bool fix;
    public float lerp;
    public Vector3 offset;
    Rigidbody2D playerRb;
    void Start()
    {
        playerRb = target.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(fix){
            return;
        }
        transform.position = target.position+offset;
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(!fix){
            return;
        }
        transform.position = (Vector3)Vector2.Lerp(transform.position,target.position, lerp)+offset;
    }
}
