using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;
public class callIKManager : MonoBehaviour
{
    public IKManager2D iKManager;

    // Update is called once per frame
    void LateUpdate()
    {
        iKManager.UpdateManager();
    }
}
