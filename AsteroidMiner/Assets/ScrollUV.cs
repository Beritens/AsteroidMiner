using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer mR;
    public Texture2D text;
    public float paralax;
    void Start()
    {
        mR = GetComponent<MeshRenderer>();
        mR.material.mainTexture = text;
    }

    // Update is called once per frame
    void Update()
    {
        mR.material.mainTextureOffset = new Vector2(transform.position.x/transform.localScale.x,transform.position.y/transform.localScale.y)/paralax;
    }
}
