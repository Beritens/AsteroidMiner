using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drillProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float damage;
    public LayerMask layerMask;
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        asteroid astro = other.gameObject.GetComponent<asteroid>();
        if(astro != null){
            astro.damage(damage);
        }
        if(layerMask == (layerMask | (1 << other.gameObject.layer)))
            Destroy(gameObject);
    }
}
