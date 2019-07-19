using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class window : selectable
{
    protected bool Open =false;
    public GameObject Window;
    public void open(){
        if(!Open){
            Open = true;
            Window.SetActive(true);
        }
            
    }
    public void close(){
        if(Open){
            Open = false;
            Window.SetActive(false);
        }
            
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && Open){
            close();
        }
    }
}
