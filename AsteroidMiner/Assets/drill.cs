using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drill : tool
{
    public float damagePerSecond;
    public Animator drillHead;
    public float drillSpeed;
    public ParticleSystem[] particles;
    public Transform head;
    public Transform testHead;
    Collider2D headcol;
    Collider2D headcolHinten;
    Collider2D headcolTest;
    public Transform rod;
    float defaultExtend;
    public float extendable = 1;
    public float extendSpeed = 0.5f;
    float currentExtend = 0;
    float targetExtend = 0;
    Vector2 oldPosition;
    public override void star(){
        defaultExtend = head.localPosition.x;
        headcol = head.GetComponent<Collider2D>();
        headcolHinten = head.GetChild(0).GetComponent<Collider2D>();
        headcolTest = testHead.GetComponent<Collider2D>();
        oldPosition = transform.TransformPoint(Vector2.right*(toolLength+currentExtend));//toolManager.focus.position;
        testHead.parent = null;
    }
    int switchWait = 0;
    //onSwitch side assign variables so you don't have to worry about the side in the calculation
    
    public override void OnSwitchSide(bool rightori){
        base.OnSwitchSide(rightori);
        toolManager.FocusForeground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        toolManager.FocusBackground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;
        lockIk(rightori? 2:3, true);
        lockIk(rightori? 3:2, false);
        toolManager.focus.GetComponent<returnToPos>().enabled = false;
        toolManager.focus.GetComponent<DistanceJoint2D>().enabled = false;
        toolManager.focus.position = HandPos(oldPosition);
         head.localScale = new Vector3(Mathf.Abs(head.localScale.x)*(rightori?1:-1),head.localScale.y,head.localScale.z);
         switchWait = 2;
        // foreach(follow f in ground.ikStuff){
        //     f.updateIt();
        // }
        LateUpdate();
        //switchS = true;
        
    } 
    void OnDestroy()
    {
        toolManager.FocusForeground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        toolManager.FocusBackground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;
        Destroy(testHead.gameObject);
    } 
    // Update is called once per frame
    bool drilling;
    bool changeOldPos;
    void LateUpdate()
    {
        changeOldPos= true;
        Debug.DrawRay(shoulder.position,Vector2.up,Color.yellow);
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        // toolManager.focus doMath();
        
        Vector2 handpos = HandPos(mousePos);
        
       //rbHead.MovePosition(rbHead.position+(Vector2.right*0.1f));
        if(Input.GetButton("Fire1")){
            toolManager.focus.position = GetCorrectedHandPos(handpos);
            drilling = true;
            drillHead.SetFloat("speed",drillSpeed);
            targetExtend = Mathf.Clamp(Vector2.Distance(shoulder.position,mousePos)-(r+R),0,extendable);
            //head.localPosition = Vector2.right*(defaultExtend+targetExtend);
        }
        else{
            toolManager.focus.position = handpos;
            drilling = false;
            targetExtend = 0;
            drillHead.SetFloat("speed",0);
        }
        Collider2D[] asteroids;
        bool touching =GetTouching(out asteroids);
        bool h = GetTouchingHinten();
        float plus = (Mathf.Sign(targetExtend-currentExtend)*Time.deltaTime)/(extendSpeed*extendable);
        bool ok = true;
        if(touching && !changeOldPos){
            float lenny = (transform.InverseTransformPoint(oldPosition).x-(toolLength+currentExtend+(plus*extendable)));
            if(lenny <0){
                currentExtend += (lenny-((plus*extendable))); 
                ok = false;
            }

        }
        if(currentExtend != targetExtend &&ok){

            float f = currentExtend/extendable;
            if(drilling){
                Debug.Log(touching);
                if((plus>0 && touching)||(plus<0 && h)){
                    ok = false;
                }
                
            }
            if(ok){
                currentExtend= Mathf.Lerp(0,extendable,Mathf.Clamp01(f+plus));
                if((plus>0)?(currentExtend>targetExtend):(currentExtend<targetExtend)){
                    currentExtend = targetExtend;
                }
                
                
            }
            
            head.localPosition = Vector2.right*(defaultExtend+currentExtend);
            rod.localScale =Vector2.one+ Vector2.up*(-0.9f+currentExtend);
            
            
        }
        if(touching&& drilling){
            foreach(ParticleSystem pS in particles){
                pS.Play();
            }
            
            foreach (Collider2D col in asteroids)
            {
                if(col!= null){
                    asteroid astro = col.GetComponent<asteroid>();
                    if(astro != null){
                        astro.damage(damagePerSecond*Time.deltaTime);
                    }
                }
                
            }
        }
        else{
            foreach(ParticleSystem pS in particles){
                pS.Stop();
            }
        }
        if(switchWait == 0 && changeOldPos)
            oldPosition =  transform.TransformPoint(Vector2.right*(toolLength+currentExtend));
        
        if(switchWait>0)
            switchWait --;
    }
    bool touching;
    Vector2 DrillPos(Vector2 hand){
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if(Vector2.Distance(mousePos,shoulder.position)>R+r){
            float dist = R+r-(toolLength-defaultExtend)+currentExtend;
            Vector2 dir= (hand-(Vector2)shoulder.position).normalized;
            return (Vector2)shoulder.position + dir*dist;
        }
        float distance = Vector2.Distance(head.position, toolManager.rightOri?ground.ikStuff[2].transform.position:ground.ikStuff[3].transform.position);
        
        return hand+toolDir*distance;
    }
    bool GetTouching(){
        Collider2D[] overlaps = new Collider2D[1];
        int count = headcol.OverlapCollider(contactFilter,overlaps);
        return count>0;
    }
    bool GetTouching(out Collider2D[] overlaps){
        overlaps = new Collider2D[5];
        int count = headcol.OverlapCollider(contactFilter,overlaps);
        return count>0;
    }
    bool GetTouchingHinten(){
        Collider2D[] overlaps = new Collider2D[1];
        int count = headcolHinten.OverlapCollider(contactFilter,overlaps);

        return count>0;
    }
    public ContactFilter2D contactFilter;
    public float drillRayArea;
    Vector2 GetCorrectedHandPos(Vector2 handpos){
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        touching = GetTouching();
        if(TestDrillPosition(DrillPos(handpos))){
            return handpos;
            //Debug.DrawRay(handpos,Vector2.up,Color.blue);
        }
        else if(touching || Vector2.Distance(mousePos,shoulder.position)<R+r|| switchWait>0){
            changeOldPos = false;
            return HandPos(oldPosition);
                
        }
        else{
            Vector2 mouseDir = (mousePos-(Vector2)shoulder.position).normalized;
            Vector2 handDir = (head.transform.position-shoulder.position).normalized;
            float dist = r+R+currentExtend;
            //Debug.DrawRay(shoulder.position,mouseDir*dist, Color.yellow,5);
            //Debug.DrawRay(shoulder.position,handDir*dist, Color.blue,5);
            
            
            for(int i = 0; i<5;i++){
                Vector2 rayDir =  ((mouseDir+handDir)/2).normalized;
                RaycastHit2D hit = Physics2D.CircleCast((Vector2)shoulder.position+rayDir*(dist-drillRayArea),0.3f,rayDir,drillRayArea,contactFilter.layerMask);
                if(hit.collider == null){
                    handDir = rayDir;
                }
                else{
                    mouseDir = rayDir;
                }
            }
            Vector2 finalDir = ((mouseDir+handDir)/2).normalized;
            //Debug.DrawRay(shoulder.position,finalDir*dist, Color.green,5);
            return (Vector2)shoulder.position+finalDir*dist;
        }
    }
    bool TestDrillPosition(Vector2 pos){
        testHead.position = pos;
        testHead.up = toolDir;
        
        Collider2D[] overlaps = new Collider2D[1];
        int count = headcolTest.OverlapCollider(contactFilter,overlaps);
        return count == 0;
    }

    //have fun future Ben :D //f*ck you past ben
}
