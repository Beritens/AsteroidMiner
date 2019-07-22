using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drill : tool
{
    Transform foregroundShoulder;
    Transform backgroundShoulder;
    Transform foregroundElbow;
    Transform backgroundElbow;
    float armLengthForeground;
    float amLengthBackground;
    float forearmForeground;
    float forearmBackground;

    // Start is called before the first frame update
    void Start()
    {
        foregroundShoulder = GameObject.Find("foregroundShoulder").transform;
        backgroundShoulder = GameObject.Find("backgroundShoulder").transform;
        foregroundElbow = foregroundShoulder.GetChild(0);
        backgroundElbow = backgroundShoulder.GetChild(0);
        // get all length for calculations
    }
    //onSwitch side assign variables so you don't have to worry about the side in the calculation

    // Update is called once per frame
    void Update()
    {
        // toolManager.focus doMath();
    }
    //Vector2 doMath(){ http://mathworld.wolfram.com/Circle-CircleIntersection.html }

    //have fun future Ben :D
}
