using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collect : MonoBehaviour
{
    public ressources ressources;
    public AsteroidBelt asteroidBelt;
    void OnCollisionEnter2D(Collision2D other)
    {
        collectStuff(other.transform);

    }
    public void collectStuff(Transform t){
        if(ressources.GetStorage() >= ressources.maxStorage)
            return;
        ressource ressource = t.GetComponent<ressource>();
        if(ressource != null){
            ressources.Add(ressource.type);
            asteroidBelt.DeleteObject(t,t.GetSiblingIndex());
            Destroy(t.gameObject);
        }
    }
}
