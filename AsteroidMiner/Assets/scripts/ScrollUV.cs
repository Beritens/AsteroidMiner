using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer mR;
    public Texture2D text;
    public float paralax;
    public bool rTW = false;
    public float worldRadius;
    public bool y;
    void Start()
    {
        mR = GetComponent<MeshRenderer>();
        mR.material.mainTexture = text;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(!rTW){
            mR.material.mainTextureOffset = new Vector2(transform.position.x/transform.localScale.x,transform.position.y/transform.localScale.y)/paralax;
            transform.rotation = Quaternion.identity;
        }
        else{
            float x = -Mathf.Atan2(transform.position.y,transform.position.x)*Mathf.Rad2Deg/360*worldRadius;
            mR.material.mainTextureOffset = new Vector2(x/transform.localScale.x,(((Vector2)transform.position).magnitude-worldRadius-10f)/transform.localScale.y)/paralax;
        }
    }
}
