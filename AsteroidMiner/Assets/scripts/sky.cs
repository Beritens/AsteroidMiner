using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class sky : MonoBehaviour
{
    MeshRenderer mR;
    public Vector2 heightRange;
    public Light2D gloabalLight;
    public Color skyCol;
    public Color spaceCol;
    // Start is called before the first frame update
    void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float height = transform.position.magnitude;
        float percent = usefullStuff.map(height,heightRange.x,heightRange.y,1,0);
        var col = mR.material.color;
        col.a= percent;
        mR.material.color = col;
        gloabalLight.color = Color.Lerp(spaceCol,skyCol,percent);
    }
}
