using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class tool : MonoBehaviour
{
    protected toolManager toolManager;
    item item;
    protected ground ground;
    protected Transform foregroundShoulder;
    protected Transform backgroundShoulder;
    protected Transform foregroundElbow;
    protected Transform backgroundElbow;
    protected float armLengthForeground;
    protected float armLengthBackground;
    protected float forearmForeground;
    protected float forearmBackground;

    public float toolLength;
    protected float R;
    protected float r;
    float foreArmLength;
    protected Transform shoulder;
    protected Camera cam;
    public SpriteRenderer[] sprites;
    public void instantiate(toolManager tM, int i){
        toolManager = tM;
        item = items.instance.itemObjects[i];
        ground = toolManager.GetComponent<ground>();
        cam = Camera.main;
        star();
        GetLengths();
    }
    public virtual void star(){
        
    }
    public void changeSpriteOrder(int pos, bool oben){
        if(oben){
            pos -= sprites.Length-1;
        }
        for(int i = 0; i< sprites.Length;i++){
            sprites[i].sortingOrder = pos -i;
        }
    }
    public virtual void OnSwitchSide(bool rightori){
        
        r = rightori? armLengthForeground:armLengthBackground;
        shoulder = rightori? foregroundShoulder:backgroundShoulder;
        R = (rightori?Vector2.Distance(foregroundElbow.position,toolManager.handForeground.position):Vector2.Distance(backgroundElbow.position,toolManager.handBackground.position))+toolLength;//( Vector2.Distance(foregroundElbow.position,rightori?toolManager.handForeground.position:toolManager.handBackground.position))+toolLength;
        foreArmLength = rightori? forearmForeground:forearmBackground;
    }
    public void lockIk(int i, bool yes){
        ground.lockIk(i,yes);
    }
    public void GetLengths(){
        foregroundShoulder = GameObject.Find("foregroundShoulder").transform;
        backgroundShoulder = GameObject.Find("backgroundShoulder").transform;
        foregroundElbow = foregroundShoulder.GetChild(0);
        backgroundElbow = backgroundShoulder.GetChild(0);
        armLengthForeground = Vector2.Distance(foregroundShoulder.position,foregroundElbow.position);
        armLengthBackground = Vector2.Distance(backgroundShoulder.position,backgroundElbow.position);
        //rigging rig = toolManager.GetComponentInChildren<rigging>();
        forearmForeground = Vector2.Distance(toolManager.ikForegroundArm.position,foregroundElbow.position);
        forearmBackground = Vector2.Distance(toolManager.ikBackgroundArm.position,backgroundElbow.position);
        OnSwitchSide(toolManager.rightOri);
    }
    public Vector2 HandPos(Vector2 mousePos){
        float d= Vector2.Distance(mousePos,shoulder.position);
        if(d> (R+r)){
            toolDir = (mousePos-(Vector2)shoulder.position).normalized;
            return mousePos;
            
        }
        else if(d <Mathf.Abs(R-r)+0.05f){
            d = Mathf.Abs(R-r)+0.05f;
            mousePos = (Vector2)shoulder.position+(mousePos-(Vector2)shoulder.position).normalized*(d);
        }
        float x = (d*d-r*r+R*R)/(2*d);
       // float a = Mathf.Sqrt((-d+r-R)*(-d-r+R)*(-d+r+R)*(d+r+R));
       float h = Mathf.Sqrt(R*R-x*x);
        
        Vector2 dir = ((Vector2)shoulder.position-mousePos).normalized;
        Vector2 elbow = mousePos+((dir*x)+(Vector2.Perpendicular(dir)*h* (toolManager.rightOri? 1:-1)));
        Vector2 elToHand = (mousePos-elbow).normalized;
        
        toolDir = elToHand;
        return  elbow+elToHand* foreArmLength;

    }
    protected Vector2 toolDir;
}
