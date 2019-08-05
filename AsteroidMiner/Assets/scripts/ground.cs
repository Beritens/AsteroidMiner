using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground : MonoBehaviour
{
    public string groundTag;
    public bool grounded;
    public bool superGrounded;
    public LayerMask lm;
    public float rayLength;
    public float rayLengthOnGround;
    public float supersuperGroundedLength;
    public Collider2D[] deactivate;
    public Collider2D[] activate;
    [HideInInspector]
    public Vector2 normal;
    [HideInInspector]
    public Vector2 point;
    public float gravityAngle = 20f;
    Rigidbody2D rb;
    public follow[] ikStuff;
    bool[] lockIks = new bool[4];
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void lockIk(int i, bool yes){
        lockIks[i]= yes;
        if(superGrounded){
            ikStuff[i].enabled = yes;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = GetGround();
        bool g = hit.collider != null;
        if(g&&transform.position.magnitude < 10900){
            float gAn = Vector2.Angle(transform.position,hit.normal);
            if(gAn>gravityAngle){
                g= false;
            }
        }
        
        normal = hit.normal;
        point = hit.point;
        if(grounded != g){
            grounded = g;
            for(int i = 0; i<ikStuff.Length; i++){
                ikStuff[i].enabled = !grounded;
                if(grounded && lockIks[i]){
                    ikStuff[i].enabled = true;
                }
            }
            
            
            if(grounded){
                superGroundedCkeck = rayLength;
                foreach(Collider2D c in deactivate){
                    c.enabled = false;
                }
                foreach(Collider2D c in activate){
                    c.enabled = true;
                }
            }
            else{
                foreach(follow f in ikStuff){
                    f.enabled = true;
                }
                foreach(Collider2D c in deactivate){
                    c.enabled = true;
                }
                foreach(Collider2D c in activate){
                    c.enabled = false;
                }
            }
        }
        if(grounded){
            bool sG = hit.distance<= superGroundedCkeck;
            if(sG != superGrounded){
                superGrounded = sG;
                for(int i = 0; i<ikStuff.Length; i++){
                    ikStuff[i].enabled = !superGrounded;
                    if(grounded && lockIks[i]){
                        ikStuff[i].enabled = true;
                    }
                }
            }
            if(superGrounded){
                superGroundedCkeck = rayLength;
                foreach(Collider2D c in deactivate){
                    c.enabled = false;
                }
                foreach(Collider2D c in activate){
                    c.enabled = true;
                }
            }
            else{
                superGrounded = false;
                superGroundedCkeck = supersuperGroundedLength;
                foreach(Collider2D c in deactivate){
                    c.enabled = true;
                }
                foreach(Collider2D c in activate){
                    c.enabled = false;
                }
            }
            
            //rb.angularVelocity = 0;
            //rb.rotation  = Mathf.Atan2(hit.normal.x,hit.normal.y)*Mathf.Rad2Deg;
            
        }
        else{
            superGrounded = false;
        }
        if(touchingGround && !grounded){
            float playerAngle =  Vector2.SignedAngle(touchnormal,transform.up);
            bool right = (playerAngle <0);
            Vector2 inputAxis = Camera.main.transform.TransformDirection(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
            float inputAngle = Vector2.SignedAngle(touchnormal,inputAxis);
            if((right && inputAngle >20 && inputAngle <160)||(!right&& inputAngle<-20 && inputAngle >-160)){
                rb.AddTorque(torque*(right?1:-1));
            }
            
        }
    }
    public float torque = 10000f;
    RaycastHit2D GetGround(){
        RaycastHit2D[] hits =  Physics2D.RaycastAll(transform.position,-transform.up,grounded? rayLengthOnGround:rayLength,lm);
        foreach(RaycastHit2D h in hits){
            if(h.collider != null && h.collider.tag == "ground"){
                return h;
            }
        }
        return new RaycastHit2D();
    }
    float superGroundedCkeck;
    bool touchingGround;
    Vector2 touchnormal;
    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "ground"){
            touchingGround = true;
            touchnormal = other.contacts[0].normal;
        }
            
    }
    void OnCollisionExit2D(Collision2D other)
    {
        touchingGround = false;
    }
}
