using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterAnim : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
         if (animator.GetCurrentAnimatorStateInfo(0).IsName("New State"))
        {
            Destroy(gameObject);
        }
    }
}
