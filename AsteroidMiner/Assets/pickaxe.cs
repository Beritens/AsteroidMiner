using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickaxe : tool
{
    Camera cam;
    AsteroidBelt belt;
    select select;
    public Color selectColor;
    public float strenth =10f;
    //look look;
    public Transform arm1;
    public Transform arm2;
    public GameObject pickaxePrefab;
    public Transform foregroundShoulder;
    public Transform backgroundShoulder;

    // Start is called before the first frame update
    void Start()
    {
        toolTrans = GameObject.Instantiate(pickaxePrefab, handForeground.position, handForeground.rotation,handForeground).transform;
        star();
        right();
        //look = GetComponent<look>();
        cam = Camera.main;
        belt = GameObject.FindObjectOfType<AsteroidBelt>().GetComponent<AsteroidBelt>();
        select = GameObject.FindObjectOfType<select>().GetComponent<select>();
        look.OnSwitchSide += OnSwitchSidePic;
        
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject obj = MouseOver();
        if(Input.GetButtonDown("Fire1")){
            startdig();
            // if(obj != null){
            //     asteroid ast = obj.GetComponent<asteroid>();
            //     if(ast != null){
            //         ast.damage(strenth);
            //     }
            // }
            
        }
        //if(obj != null ){
            //select.selectObject(obj,selectColor);
            // if(Input.GetButtonDown("Fire1")){
            //     asteroid ast = obj.GetComponent<asteroid>();
            //     if(ast != null){
            //         ast.damage(strenth);
            //     }
            // }
            
        //}
        //else{
          //  select.deselect();
       // }
    }
    public void OnSwitchSidePic(bool rightori){
        FocusForeground.GetComponent<returnToPos>().enabled = true;
        FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        FocusBackground.GetComponent<returnToPos>().enabled = true;
        FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;
        if(animatePickIsRunning){
            focus.GetComponent<returnToPos>().enabled = false;
            focus.GetComponent<DistanceJoint2D>().enabled = false;
        }
        
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
        if(!rightOri){
            shoulderPos = backgroundShoulder.position;
        }
        float dist;
        GameObject obj = MouseOver(shoulderPos,out dist);
        Debug.Log(dist);
        if(obj == null)
            return;
        
        if(dist<= reach){
            asteroid ast = obj.GetComponent<asteroid>();
                if(ast != null){
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
        focus.GetComponent<returnToPos>().enabled = false;
        focus.GetComponent<DistanceJoint2D>().enabled = false;
        
        float animtime = 0;
            
        while(animtime <= raiseDuration){
            animtime += Time.deltaTime;
            focus.localPosition = Vector2.Lerp(focus.localPosition, Vector2.up, animtime/raiseDuration);
            yield return null;
        }
        while(animtime <= swingDur+raiseDuration){
            animtime += Time.deltaTime;
            focus.position = Vector2.Lerp(focus.position, GetWantedHandPos(), (animtime-raiseDuration)/swingDur);
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
                focus.GetComponent<returnToPos>().enabled = true;
                focus.GetComponent<DistanceJoint2D>().enabled = true;
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
}
