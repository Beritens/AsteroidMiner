using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look : MonoBehaviour
{
    public Transform headlight;
    public Transform guy;
    public Transform head;
    float defaultHeadRotation;
    Camera cam;
    public Vector2 headLimit;
    public rigging rig;
    public pickaxe pic;
    public delegate void OnSwitchSideHandler(bool right);
    public event OnSwitchSideHandler OnSwitchSide;
 
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        defaultHeadRotation = head.transform.eulerAngles.z;
        
    }
    public bool right = true;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousepos-(Vector2)head.position;
        headlight.up = direction;
        //float angle = Mathf.Atan2(direction.x,direction.y)* Mathf.Rad2Deg;
        
        if(transform.InverseTransformPoint(mousepos).x <0 && right){
            right = false;
            tellLeft();
            
            guy.localScale = new Vector3(guy.localScale.x *-1, guy.localScale.y, guy.localScale.z);
        }
        else if(transform.InverseTransformPoint(mousepos).x >0 && !right){
            right = true;
            tellRight();
            
            guy.localScale = new Vector3(guy.localScale.x *-1, guy.localScale.y, guy.localScale.z);
        }
        // if(right){
        float headBodyAngle= Mathf.Clamp(Vector2.SignedAngle((right? 1:-1)*transform.right,direction),headLimit.x,headLimit.y);
        float bodyAngle = transform.eulerAngles.z;
        head.eulerAngles = Vector3.forward * (headBodyAngle+bodyAngle);
            
        // }
        // else{
        //     float headBodyAngle= Mathf.Clamp(Vector2.SignedAngle(-transform.right,direction),headLimit.x,headLimit.y);
        //     float bodyAngle = transform.eulerAngles.z;
        //     head.eulerAngles = Vector3.forward * (headBodyAngle+bodyAngle);
        // }
        
        
    }
    void tellRight(){
        if(OnSwitchSide != null){
            OnSwitchSide(true);
        }
        rig.right();
    }
    void tellLeft(){
        if(OnSwitchSide != null){
            OnSwitchSide(false);
        }
        rig.left();
        //pic.left();
    }
}
