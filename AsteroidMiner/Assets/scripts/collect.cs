using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collect : MonoBehaviour
{
    inventory inventory;
    public AsteroidBelt asteroidBelt;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        inventory = inventory.instance;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        collectStuff(other.transform);

    }
    public void collectStuff(Transform t){
        ressource ressource = t.GetComponent<ressource>();
        if(ressource != null){
            bool ok = inventory.AddToSlots(ressource.item);
            if(ok)
                asteroidBelt.DeleteItem(t);
            //Destroy(t.gameObject);
        }
    }
}
