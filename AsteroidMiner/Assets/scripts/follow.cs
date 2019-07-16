using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform target;
    public bool fix;
    public float smoothTime;
    public Vector3 offset;
    Rigidbody2D playerRb;
    Vector3 velocity = Vector3.zero;

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
    void LateUpdate()
    {
        if(!fix){
            return;
        }
        //transform.position = Vector3.SmoothDamp(transform.position,target.position+offset,ref velocity, smoothTime);
        transform.position = target.position+offset;
        //transform.position = (Vector3)Vector2.Lerp(transform.position,target.position, lerp*Time.deltaTime)+offset;
    }
}
