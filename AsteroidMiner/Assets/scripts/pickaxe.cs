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
    public GameObject pickaxePrefab;
    public Transform foregroundShoulder;
    public Transform backgroundShoulder;
    Vector2 localShoulderPos;
    public ParticleSystem pickaxeHit;

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
        pickaxeHit.gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        float lol;
        localShoulderPos = transform.InverseTransformPoint(foregroundShoulder.position);
        if(!rightOri){
            localShoulderPos = transform.InverseTransformPoint(backgroundShoulder.position);
        }
        GameObject obj = MouseOver(transform.TransformPoint(localShoulderPos),out lol);
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
        if(obj == null)
            return;
        
        if(dist<= reach){
            asteroid ast = obj.GetComponent<asteroid>();
                pickaxeHit.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
                pickaxeHit.Play();
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
            Vector2 direction = ((Vector2)transform.InverseTransformPoint(GetWantedHandPos())-localShoulderPos);
            //Debug.Log((Quaternion.Euler(0,0,90*(rightOri? 1: -1))*direction).normalized.magnitude);
            focus.localPosition = Vector2.Lerp(focus.localPosition, localShoulderPos+(Vector2)((Quaternion.Euler(0,0,80*(rightOri? 1: -1))*direction).normalized*0.4f), animtime/raiseDuration);
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
