using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolManager : MonoBehaviour
{
    Sprite foregroundArm;
    Sprite backgroundArm;
    [HideInInspector]
    public Transform focus;
    public Transform FocusForeground;
    public Transform FocusBackground;
    public SpriteRenderer ForegroundArm;
    public SpriteRenderer BackgroundArm;
    public Sprite foregroundArmHolding;
    public Sprite backgroundArmHolding;
    public look look;
    public Transform handForeground;
    public Transform handBackground;
    public Transform effectsContainer;
    public int orderInLayer1 = 3;
    public int orderInLayer2 = -2;
    [HideInInspector]
    public GameObject currentTool;
    [HideInInspector]
    public tool currentToolTool;
    [HideInInspector]
    public bool rightOri = true;
    SpriteRenderer[] ToolspriteRenderer;
    items items;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        look.OnSwitchSide += OnSwitchSide;
        foregroundArm = ForegroundArm.sprite;
        backgroundArm = BackgroundArm.sprite;
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        items = items.instance;
    }
    public void OnSwitchSide(bool rightori){
        if(this.enabled == false)
            return;
        rightOri = rightori;
        if(currentTool != null){
            currentToolTool.OnSwitchSide(rightori);
        }
        if(rightori){
            right();
        }
        else{
            left();
        }
    }
    public void selectTool(int item){
        currentTool = GameObject.Instantiate(items.itemObjects[item].prefab,transform.position,Quaternion.identity);
        currentToolTool = currentTool.GetComponent<tool>();
        currentToolTool.instantiate(this,item);
        ToolspriteRenderer = currentTool.GetComponentsInChildren<SpriteRenderer>();
        if(rightOri){
            right();
        }
        else{
            left();
        }
    }
    public void deselectTool(){
         BackgroundArm.sprite = backgroundArm;
        ForegroundArm.sprite = foregroundArm;
        Destroy(currentTool);
    }
    public void left(){
        focus = FocusBackground;
        if(currentTool == null)
            return;
        currentTool.transform.parent = handBackground;
        currentTool.transform.localPosition = Vector2.zero;
        currentTool.transform.localRotation = Quaternion.identity;
        ForegroundArm.sprite = foregroundArm;
        BackgroundArm.sprite = backgroundArmHolding;
        foreach(SpriteRenderer s in ToolspriteRenderer){
            s.sortingOrder = orderInLayer2;
        }
        

        
    }
    public void right(){
        focus = FocusForeground;
        if(currentTool == null)
            return;
        currentTool.transform.parent = handForeground;
        currentTool.transform.localPosition = Vector2.zero;
        currentTool.transform.localRotation = Quaternion.identity;
        BackgroundArm.sprite = backgroundArm;
        ForegroundArm.sprite = foregroundArmHolding;
        foreach(SpriteRenderer s in ToolspriteRenderer){
            s.sortingOrder = orderInLayer1;
        }

        
    }
}
