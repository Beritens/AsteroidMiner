using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickaxe : tool
{
    select select;
    public Color selectColor;
    public float strenth =10f;
    Vector2 localShoulderPos;
    ParticleSystem pickaxeHit;

    // Start is called before the first frame update
    void Start()
    {
        //foregroundShoulder = GameObject.Find("foregroundShoulder").transform;
        //backgroundShoulder = GameObject.Find("backgroundShoulder").transform;
        //look = GetComponent<look>();
        select = GameObject.FindObjectOfType<select>().GetComponent<select>();
        pickaxeHit = toolManager.effectsContainer.GetChild(0).GetComponent<ParticleSystem>();
        pickaxeHit.gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        float lol;
        localShoulderPos = toolManager.transform.InverseTransformPoint(foregroundShoulder.position);
        if(!toolManager.rightOri){
            localShoulderPos = toolManager.transform.InverseTransformPoint(backgroundShoulder.position);
        }
        GameObject obj = MouseOver(toolManager.transform.TransformPoint(localShoulderPos),out lol);
        if(Input.GetButtonDown("Fire1")){
            startdig();
            // if(obj != null){
            //     asteroid ast = obj.GetComponent<asteroid>();
            //     if(ast != null){
            //         ast.damage(strenth);
            //     }
            // }
            
        }
        if(obj != null ){
            if(lol <= reach)
                select.selectObject(obj,selectColor);
            else{
                select.deselect();
            }
            // else if(lol > reach)
            //     select.selectObject(obj,selectColorOOR);
            // if(Input.GetButtonDown("Fire1")){
            //     asteroid ast = obj.GetComponent<asteroid>();
            //     if(ast != null){
            //         ast.damage(strenth);
            //     }
            // }
            
        }
        else{
           select.deselect();
        }
    }
    public override void OnSwitchSide(bool rightori){
        toolManager.FocusForeground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        toolManager.FocusBackground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;
        
        if(animatePickIsRunning){
            lockIk(rightori? 2:3, true);
            lockIk(rightori? 3:2, false);
            toolManager.focus.GetComponent<returnToPos>().enabled = false;
            toolManager.focus.GetComponent<DistanceJoint2D>().enabled = false;
        }
        
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        toolManager.FocusForeground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        toolManager.FocusBackground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;
        if(pickaxeHit != null)
            pickaxeHit.gameObject.SetActive(false);
    }
    public float reach = 1;
    void startdig(){
        if(animatePickIsRunning){
            return;
            // dig();
            //StartCoroutine(animatePick(0.175f));
        }
        else{
            StartCoroutine(animatePick());
        }
        
    }
    void dig(){
        
        Vector2 shoulderPos = foregroundShoulder.position;
        if(!toolManager.rightOri){
            shoulderPos = backgroundShoulder.position;
        }
        float dist;
        GameObject obj = MouseOver(shoulderPos,out dist);
        if(obj == null || obj.transform.GetComponent<Collider2D>() == null)
            return;
       // Vector2 nearestPoint = obj.transform.GetComponent<Collider2D>().ClosestPoint(shoulderPos);
        
        
        if(dist<= reach){
            asteroid ast = obj.GetComponent<asteroid>();
            if(ast != null){
                pickaxeHit.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
                pickaxeHit.Play();
                ast.damage(strenth);
            }   
                
            
        }
        //StartCoroutine("animatePick");
    }
    float raiseDuration = 0.2f;
    float swingDur = 0.1f;
    bool animatePickIsRunning = false;
    IEnumerator animatePick(){
        animatePickIsRunning = true;
        toolManager.focus.GetComponent<returnToPos>().enabled = false;
        toolManager.focus.GetComponent<DistanceJoint2D>().enabled = false;
        lockIk(toolManager.rightOri? 2:3, true);
        float animtime = 0;
            
        while(animtime <= raiseDuration){
            animtime += Time.deltaTime;
            Vector2 direction = ((Vector2)toolManager.transform.InverseTransformPoint(GetWantedHandPos())-localShoulderPos);
            //Debug.Log((Quaternion.Euler(0,0,90*(rightOri? 1: -1))*direction).normalized.magnitude);
            toolManager.focus.localPosition = Vector2.Lerp(toolManager.focus.localPosition, localShoulderPos+(Vector2)((Quaternion.Euler(0,0,80*(toolManager.rightOri? 1: -1))*direction).normalized*0.4f), animtime/raiseDuration);
            yield return null;
        }
        while(animtime <= swingDur+raiseDuration){
            animtime += Time.deltaTime;
            toolManager.focus.position = Vector2.Lerp(toolManager.focus.position, GetWantedHandPos(), (animtime-raiseDuration)/swingDur);
            yield return null;
        }
        dig();
        if(Input.GetButton("Fire1")){
            StartCoroutine(animatePick());
        }
        else{

            animatePickIsRunning = false;
            yield return new WaitForSeconds(0.1f);
            if(!animatePickIsRunning){
                toolManager.focus.GetComponent<returnToPos>().enabled = true;
                toolManager.focus.GetComponent<DistanceJoint2D>().enabled = true;
                lockIk(2, false);
                lockIk(3, false);
            }
            
        }
        
    }
    Vector2 GetWantedHandPos(){
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        return mousePos;
    }
    GameObject MouseOver(Vector2 shoulderPos, out float distance){
        RaycastHit2D[] hit = Physics2D.RaycastAll(cam.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
        if(hit.Length != 0){
            
            GameObject thisOne = null;
            distance = 1000000000;
            for(int i = 0; i<hit.Length;i++){
                
                if(hit[i].transform.tag == "asteroid"){
                    Vector2 nearestPoint = hit[i].transform.GetComponent<Collider2D>().ClosestPoint(shoulderPos);
                    float dist =Vector2.Distance(shoulderPos,nearestPoint);
                    
                    if(dist < distance){
                        distance = dist;
                        thisOne = hit[i].transform.gameObject;
                    }
                    
                }
                
                
            }
            return thisOne;
        }
        else{
            distance = 0;
            return null;
        }
        
    }
    // void IKStuffOnGround(int fol, bool enabled){
    //     if(ground.superGrounded){
    //         ground.ikStuff[fol].enabled = enabled;
    //     }

    // }
}
