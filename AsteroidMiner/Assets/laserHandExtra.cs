using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserHandExtra : MonoBehaviour
{
    laserHand laserHand;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        laserHand = GetComponent<laserHand>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        laserHand.laserStuff();
    }
}
