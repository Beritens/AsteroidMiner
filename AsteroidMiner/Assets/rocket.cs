using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : selectable
{
    bool active = false;
    Transform player;
    Rigidbody2D rb;
    public Vector2 cOM;
    public Vector2 upEngine;
    public Vector2 rightEngine;
    public Vector2 leftEngine;
    public float steeringAngle;
    public float speed;
    public float steeringSpeed;
    public GameObject rightFire;
    public GameObject leftFire;
    public GameObject upFire;
    Camera cam;
    Transform background;
    public ParticleSystem grind;
    public override void pressE(){
        active = true;
        player.parent = transform;
        player.localPosition = Vector2.zero;
        player.gameObject.SetActive(false);
        cam.orthographicSize = 25;
        background.localScale = new Vector3(5,5,1);

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    Vector2 right;
    Vector2 left;
    void Start()
    {
        player = GameObject.FindObjectOfType<look>().transform;
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = cOM;
        right = steeringAngle.DegreeToVector2();
        left = new Vector2(-right.x,right.y);
        cam = Camera.main;
        background = GameObject.Find("background").transform;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(active){
            float y = Mathf.Clamp01(Input.GetAxisRaw("Vertical"));
            float x = Input.GetAxisRaw("Horizontal");
            if(y!= 0)
                upFire.SetActive(true);
            else   
                upFire.SetActive(false);
            rb.AddForceAtPosition(transform.up*speed*y,transform.TransformPoint(upEngine));
            Debug.DrawRay(transform.TransformPoint(rightEngine),transform.TransformDirection(right));
            Debug.DrawRay(transform.TransformPoint(leftEngine),transform.TransformDirection(left));
            Debug.DrawRay(transform.TransformPoint(upEngine),transform.up);

            if(x>0){
                rb.AddForceAtPosition(transform.TransformDirection(left)*steeringSpeed*x,transform.TransformPoint(leftEngine));
                rightFire.SetActive(true);
                leftFire.SetActive(false);
            }
            else if(x<0){
                rb.AddForceAtPosition(transform.TransformDirection(right)*steeringSpeed*-x,transform.TransformPoint(rightEngine));
                rightFire.SetActive(false);
                leftFire.SetActive(true);
            }
            else{
                rightFire.SetActive(false);
                leftFire.SetActive(false);
            }


        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if(rb.velocity.magnitude >= 10f){
            grind.Play();
            //var emis = grind.emission;
            //emis.rateMultiplier = rb.velocity.magnitude;
            //grind.transform.right= -rb.velocity;
            //var main = grind.main;
            //main.startSpeedMultiplier = rb.velocity.magnitude;
            Vector2 point = Vector2.zero;
            float prevAngle = 360;
            foreach(ContactPoint2D contact in other.contacts){
                Vector2 localPoint = transform.InverseTransformPoint(contact.point);
                float angle = Vector2.Angle(transform.InverseTransformDirection(rb.velocity),localPoint);
                if(angle < prevAngle){
                    prevAngle = angle;
                    point = contact.point;
                }
            }
            grind.transform.position =point+rb.velocity*Time.deltaTime;
        }
            
        else
            grind.Stop();
        
    }
    void OnCollisionExit2D(Collision2D other)
    {
        grind.Stop();
    }
}
