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
    public follow foregroundIk;
    public follow backgroundIK;
    public Transform leftIK;
    public Transform rightIK;
    public follow foregroundIkArm;
    public follow backgroundIKArm;
    public Transform leftIKArm;
    public Transform rightIKArm;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        defaultHeadRotation = head.transform.eulerAngles.z;
    }
    bool right = true;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousepos-(Vector2)head.position;
        headlight.up = direction;
        //float angle = Mathf.Atan2(direction.x,direction.y)* Mathf.Rad2Deg;
        
        if(transform.InverseTransformPoint(mousepos).x <0 && right){
            right = false;
            foregroundIk.target = rightIK;
            backgroundIK.target = leftIK;
            foregroundIkArm.target = rightIKArm;
            backgroundIKArm.target = leftIKArm;
            guy.localScale = new Vector3(guy.localScale.x *-1, guy.localScale.y, guy.localScale.z);
        }
        else if(transform.InverseTransformPoint(mousepos).x >0 && !right){
            right = true;
            foregroundIk.target = leftIK;
            backgroundIK.target = rightIK;
            foregroundIkArm.target = leftIKArm;
            backgroundIKArm.target = rightIKArm;
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
}
