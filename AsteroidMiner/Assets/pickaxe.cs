using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickaxe : MonoBehaviour
{
    Camera cam;
    AsteroidBelt belt;
    select select;
    public Color selectColor;
    public float strenth =10f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        belt = GameObject.FindObjectOfType<AsteroidBelt>().GetComponent<AsteroidBelt>();
        select = GameObject.FindObjectOfType<select>().GetComponent<select>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = MouseOver();
        if(obj != null ){
            select.selectObject(obj,selectColor);
            if(Input.GetButtonDown("Fire1")){
                asteroid ast = obj.GetComponent<asteroid>();
                if(ast != null){
                    ast.damage(strenth);
                }
            }
            
        }
        else{
            select.deselect();
        }
    }
    GameObject MouseOver(){
        RaycastHit2D[] hit = Physics2D.RaycastAll(cam.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
        if(hit.Length != 0){
            for(int i = 0; i<hit.Length;i++){
                if(hit[i].transform.tag == "asteroid"){
                    return hit[i].transform.gameObject;
                }
                
            }
            return null;
        }
        else{
            return null;
        }
        
    }
}
