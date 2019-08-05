using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drillGun : tool
{
    public GameObject projectile;
    public Transform position;
    public float damage;
    public float force;
    public float speed;
    Rigidbody2D player;
    public Animator anim;
    public override void star(){
        player = toolManager.GetComponent<Rigidbody2D>();
    }
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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        toolManager.focus.position = HandPos(mousePos);
        
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    bool spawn;
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            spawn = true;
        }
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(!spawn)
            return;
        
        spawn = false;
        GameObject g = GameObject.Instantiate(projectile,position.position,Quaternion.identity);
        g.transform.up = transform.right* (toolManager.rightOri?1:-1);
        drillProjectile dr = g.GetComponent<drillProjectile>();
        dr.damage = damage;
        
        dr.rb.velocity += player.velocity;
        dr.rb.AddForce((Vector2)(g.transform.up*force),ForceMode2D.Impulse);
        dr.GetComponent<Animator>().SetFloat("speed",speed);
        anim.Play("reload");
    }
}
