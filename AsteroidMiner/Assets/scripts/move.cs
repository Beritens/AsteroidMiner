using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class move : MonoBehaviour
{
    public float speed =1f;
    public float angularSpeed =1f;
    Rigidbody2D rb;
    [Header("ground")]
    public ground ground;
    public Animator anim;
    public float groundSpeed;
    public float jumpForce;
    public look look;
    public float groundDamp= 0.9f;

    [Header("jets")]
    public Light2D[] jetLights;
    public ParticleSystem[] jetParticles;
    public bool[]  down = new bool[32];
    public bool[]  right = new bool[32];
    public bool[] left = new bool[32];
    //public bool[]up = new bool[32];
    public bool[] up = new bool[32];
    
    
    int uI = 0;
    int dI =0;
    int rI = 0;
    int lI = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public int AddUp(){
        uI++;
        up[uI]=false;
        return(uI);
    }
    public int AddDown(){
        dI++;
        down[dI]=false;
        return(dI);
    }
    public int AddRight(){
        rI++;
        right[rI]=false;
        return(rI);
    }
    public int AddLeft(){
        lI++;
        left[lI]=false;
        return(lI);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public bool g= false;
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xraw = Input.GetAxisRaw("Horizontal");
        float yraw = Input.GetAxisRaw("Vertical");
        if(!ground.grounded){
            if(g){
                anim.enabled = false;
                rb.angularDrag = 0f;
            }
            g= false;
            
            #region 
            if(xraw>0){
                right[0]= true;
                left[0]= false;
            }
            else if(xraw<0){
                right[0]= false;
                left[0]= true;
            }
            else{
                right[0]= false;
                left[0]= false;
            }
            if(yraw>0){
                up[0]= true;
                down[0]= false;
            }
            else if(yraw<0){
                up[0]= false;
                down[0]= true;
            }
            else{
                up[0]= false;
                down[0]= false;
            }
            #endregion
            
            Vector2 direction = new Vector2(x,y).Rotate(rb.rotation);
            //rb.velocity=(transform.up * y * speed);
            BoostUp(y);
            Rotate(-x);
            //rb.angularVelocity = Mathf.Clamp(rb.angularVelocity,-maxAngularVelocity,maxAngularVelocity);
            //transform.position += new Vector3(x,y,0)*speed;
            jets();
            //later an upgarde
            if(Input.GetButton("slowDown")){
                rb.velocity *= 0.98f;
                rb.angularVelocity *= 0.98f;
            }
        }
        else{
            if(!g){
                anim.enabled = true;
                g= true;
                right[0]= false;
                left[0]= false;
                up[0]= false;
                down[0]= false;
                rb.angularDrag = 100f;
                
                rb.rotation = Mathf.Atan2(ground.normal.y,ground.normal.x)* Mathf.Rad2Deg-90;
                float horizontalVel = transform.InverseTransformDirection(rb.velocity).x;
                rb.velocity = ground.normal.Rotate(-90)*horizontalVel;
                Debug.DrawRay(transform.position,rb.velocity,Color.red,10);
                
                //rb.velocity = Vector2.zero;
                
            }
            rb.rotation = Mathf.Atan2(ground.normal.y,ground.normal.x)* Mathf.Rad2Deg-90;

            jets();
            moveGround(x,yraw);
        }
    }
    public void moveGround(float x, float y){
        //rb.AddForce(transform.right*x*groundSpeed);
        if(y> 0 && ground.superGrounded){
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
        }
        float horizontalVel = transform.InverseTransformDirection(rb.velocity).x;
        rb.velocity -= (Vector2)transform.right*horizontalVel;
        rb.velocity += (Vector2)transform.right*(x*groundSpeed+(horizontalVel*groundDamp));
        if(ground.superGrounded){
            anim.enabled = true;
            anim.SetFloat("speed",horizontalVel*(look.right?1:-1));
            if(Mathf.Abs(horizontalVel)>0.005f || x != 0){
                anim.SetBool("walk",true);

            }
            else{
                anim.SetBool("walk",false);
            }
        }
        else{
            anim.enabled = false;
        }
    }
    public void BoostUp(float power){
        rb.AddForce(transform.up * power * speed);
    }
    public void Rotate(float power){
        rb.AddTorque(power*angularSpeed);
    }
    public void jets(){
        bool upJ = false;
        bool downJ = false;
        bool rightJ = false;
        bool leftJ = false;
        foreach(bool b in up){
            if(b)
                upJ = true;
        }
        foreach(bool b in down){
            if(b)
                downJ = true;
        }
        foreach(bool b in left){
            if(b)
                leftJ = true;
        }
        foreach(bool b in right){
            if(b)
                rightJ = true;
        }
        if(rightJ){
            EnableJet(0);
            EnableJet(2);
        }
        else{
            DisableJet(0);
            DisableJet(2);
        }
        if(leftJ){
            EnableJet(1);
            EnableJet(3);
        }
        else{
            DisableJet(1);
            DisableJet(3);
        }
        
        if(upJ){
            EnableJet(4);
            EnableJet(5);
        }
        else{
            DisableJet(4);
            DisableJet(5);
        }
        if(downJ){
            EnableJet(6);
        }
        else{
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
