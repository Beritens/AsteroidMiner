using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookToMouse : MonoBehaviour
{
    public move move;
    float speed;
    Rigidbody2D rb;
    int indexR;
    int indexL;
    bool on = true;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        indexR = move.AddRight();
        indexL = move.AddLeft();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetButtonDown("orientate")){
            on = !on;
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {

        if(on && !move.g){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos+Vector2.up*0.05f)-(Vector2)transform.position;
            float angle = Vector2.SignedAngle(transform.up,direction);
            move.Rotate(Mathf.Clamp(angle/3,-1,1));
            rb.angularDrag = Mathf.Abs(rb.angularVelocity/5f)-Mathf.Abs(angle);
            if(Mathf.Abs(angle) <= 10f && Mathf.Abs(rb.angularVelocity)< 10f){
                move.right[indexR] = false;
                move.left[indexL] = false;
            }
            else{
                bool right = -(angle -  Mathf.Sign(angle)*(Mathf.Abs(rb.angularVelocity/5f)-Mathf.Abs(angle)))>=0;
                move.right[indexR] = right;
                move.left[indexL] = !right;
            }
        }
        else{
            if(!move.g){
                rb.angularDrag = 0f;
            }
            
            move.right[indexR] = false;
            move.left[indexL] = false;
        }
        
    }
}
