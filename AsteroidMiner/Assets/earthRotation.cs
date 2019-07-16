using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthRotation : MonoBehaviour
{
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        float angle = Mathf.Atan2(pos.y,pos.x)*Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0,0,angle-90);
    }
}
