using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid : MonoBehaviour
{
    public float health;
    float h;
    public Sprite[] stages;
    public float dropCircle = 1;
    public Vector2Int dropCountRange;
    SpriteRenderer r;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        h= health;
    }
    public bool damage(float amount){
        if(r == null)
            r = GetComponent<SpriteRenderer>();
        
        h-= amount;
        if(h <= 0){
            destroy();
            return true;
        }
        int index = Mathf.FloorToInt((stages.Length+1)-(h/health)*(stages.Length+1));
        if(index == 0){
            return false;
        }
        r.sprite = stages[index-1];
        return false;
        
        
    }
    void destroy(){
        AsteroidBelt ab = GameObject.FindObjectOfType<AsteroidBelt>().GetComponent<AsteroidBelt>();
        ab.DeleteObject(transform,transform.GetSiblingIndex());
        int count = Random.Range(dropCountRange.x,dropCountRange.y);
        while( count > 0){
            count--;
            //AsteroidBelt.Object drop = new AsteroidBelt.Object((transform.position+Random.insideUnitSphere*dropCircle)-transform.parent.position,1,Random.Range(0f,360f),Random.Range(0.8f,1.2f),false);
            //ab.AddObject(drop,transform.parent.gameObject,true);
            ab.AddItem(1,(Vector2)(transform.position+Random.insideUnitSphere*dropCircle),Random.Range(0f,360f),Random.Range(0.8f,1.2f));
        }
        

        Destroy(gameObject);
    }
}
