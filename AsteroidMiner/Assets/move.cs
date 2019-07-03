using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class move : MonoBehaviour
{
    public float speed =1f;
    public float angularSpeed =1f;
    Rigidbody2D rb;

    [Header("jets")]
    public Light2D[] jetLights;
    public ParticleSystem[] jetParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xraw = Input.GetAxisRaw("Horizontal");
        float yraw = Input.GetAxisRaw("Vertical");
        
        Vector2 direction = new Vector2(x,y).Rotate(rb.rotation);
        //rb.velocity=(transform.up * y * speed);
        rb.AddForce(transform.up * y * speed);
        rb.AddTorque(-x *angularSpeed);
        //rb.angularVelocity = Mathf.Clamp(rb.angularVelocity,-maxAngularVelocity,maxAngularVelocity);
        //transform.position += new Vector3(x,y,0)*speed;
        jets(xraw,yraw);
        //later an upgarde
        if(Input.GetButton("slowDown")){
            rb.velocity *= 0.98f;
            rb.angularVelocity *= 0.98f;
        }
    }
    void jets(float x, float y){
        if(x!= 0){
            if(x>0){
                EnableJet(0);
                DisableJet(1);
                EnableJet(2);
                DisableJet(3);
            }
            else{
                DisableJet(0);
                EnableJet(1);
                DisableJet(2);
                EnableJet(3);
            }
        }
        else{
            DisableJet(0);
            DisableJet(1);
            DisableJet(2);
            DisableJet(3);
        }
        if(y!=0){
        if(y>0){
                EnableJet(4);
                EnableJet(5);
                DisableJet(6);
            }
            else{
                DisableJet(4);
                DisableJet(5);
                EnableJet(6);
            }
        }
        else{
            DisableJet(4);
            DisableJet(5);
            DisableJet(6);
        }
    }
    void EnableJet(int i){
        jetLights[i].enabled = true;
        jetParticles[i].Play();
    }
    void DisableJet(int i){
        jetLights[i].enabled = false;
        jetParticles[i].Stop();
    }
}
