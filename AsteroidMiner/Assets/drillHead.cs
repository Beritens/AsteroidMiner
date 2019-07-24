using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drillHead : MonoBehaviour
{
    // Start is called before the first frame update
    drill drill;
    void Start()
    {
        drill = GetComponentInParent<drill>();
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     drill.touching = true;

    // }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     drill.touching = false;
    // }
}
