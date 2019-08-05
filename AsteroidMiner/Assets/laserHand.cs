using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserHand : tool
{
    public GameObject foreground;
    public GameObject background;
    public Transform laserOrigin;
    public GameObject laser;
    public LineRenderer[] laserBeams;
    public Transform end;
    public LayerMask layerMask;
    public GameObject explosion;
    public float damagePerSecond = 100f;
    public override void OnSwitchSide(bool rightori){
        foreground.SetActive(rightori);
        background.SetActive(!rightori);
        base.OnSwitchSide(rightori);
        toolManager.FocusForeground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        toolManager.FocusBackground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;
        lockIk(rightori? 2:3, true);
        lockIk(rightori? 3:2, false);
        toolManager.focus.GetComponent<returnToPos>().enabled = false;
        toolManager.focus.GetComponent<DistanceJoint2D>().enabled = false;
        
    } 
    void OnDestroy()
    {
        toolManager.FocusForeground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusForeground.GetComponent<DistanceJoint2D>().enabled = true;
        toolManager.FocusBackground.GetComponent<returnToPos>().enabled = true;
        toolManager.FocusBackground.GetComponent<DistanceJoint2D>().enabled = true;

    }
    void LateUpdate()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        toolManager.focus.position = HandPos(mousePos);
        
        
    }
    public void laserStuff()
    {
        if(Input.GetButton("Fire1")){
            laser.SetActive(true);
            RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position,toolDir,100,layerMask);
            if(hit.collider != null){
                foreach(LineRenderer l in laserBeams){
                    l.SetPosition(1,l.transform.InverseTransformPoint(hit.point));
                    end.position = hit.point;
                    end.up = hit.normal;
                    end.gameObject.SetActive(true);
                }
                asteroid astro = hit.collider.GetComponent<asteroid>();
                if(astro!= null){
                    Vector2 pos = astro.transform.position;
                    bool boom = astro.damage(damagePerSecond*Time.deltaTime);
                    if(boom){

                        GameObject.Instantiate(explosion,pos,Quaternion.Euler(0,0,Random.Range(0,360)));
                    }
                }
            }
            else{
                foreach(LineRenderer l in laserBeams){
                    l.SetPosition(1,Vector2.right*100f);
                    end.gameObject.SetActive(false); 
                }
            }
        }
        else{
            laser.SetActive(false);
        }
        
    }
    
}
