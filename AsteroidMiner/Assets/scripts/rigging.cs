using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigging : MonoBehaviour
{
    public follow foregroundIk;
    public follow backgroundIK;
    public Transform leftIK;
    public Transform rightIK;
    public follow foregroundIkArm;
    public follow backgroundIKArm;
    public Transform leftIKArm;
    public Transform rightIKArm;
    public Transform leftIKArm2;
    public Transform foregroundHandBone;
    public Transform backgroundHandBone;
    public Transform rightIKArm2;
    bool rightOri = true;
    // Start is called before the first frame update
    public void left(){
        rightOri = false;
        foregroundIk.target = rightIK;
        backgroundIK.target = leftIK;
        // rightIKArm2.gameObject.SetActive(true);
        // leftIKArm2.gameObject.SetActive(true);
        // rightIKArm.gameObject.SetActive(false);
        // leftIKArm.gameObject.SetActive(false);
        foregroundIkArm.target = rightIKArm2;
        backgroundIKArm.target = leftIKArm2;
    }
    public void right(){
        rightOri = true;
        foregroundIk.target = leftIK;
        backgroundIK.target = rightIK;
        // rightIKArm.gameObject.SetActive(true);
        // leftIKArm.gameObject.SetActive(true);
        // rightIKArm2.gameObject.SetActive(false);
        // leftIKArm2.gameObject.SetActive(false);
        foregroundIkArm.target = leftIKArm;
        backgroundIKArm.target = rightIKArm;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(rightOri){
            foregroundHandBone.transform.localRotation = Quaternion.Euler(0,0,leftIKArm.transform.eulerAngles.z);
            backgroundHandBone.transform.localRotation = Quaternion.Euler(0,0,rightIKArm.transform.eulerAngles.z);
        }
        else {
            foregroundHandBone.transform.localRotation = Quaternion.Euler(0,0,rightIKArm2.transform.eulerAngles.z);
            backgroundHandBone.transform.localRotation = Quaternion.Euler(0,0,leftIKArm2.transform.eulerAngles.z);
        }
    }

}
