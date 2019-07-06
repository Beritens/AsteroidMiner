using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tool : MonoBehaviour
{
    Sprite foregroundArm;
    Sprite backgroundArm;
    protected Transform focus;
    public Transform FocusForeground;
    public Transform FocusBackground;
    public SpriteRenderer ForegroundArm;
    public SpriteRenderer BackgroundArm;
    public Sprite foregroundArmHolding;
    public Sprite backgroundArmHolding;
    public look look;
    public Transform handForeground;
    public Transform handBackground;
    public int OrderInLayer1 = 3;
    public int orderInLayer2 = -2;
    protected Transform toolTrans;
    protected bool rightOri;
    SpriteRenderer ToolspriteRenderer;
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
    public void star(){
        ToolspriteRenderer = toolTrans.GetComponentInChildren<SpriteRenderer>();
    }
    public void OnSwitchSide(bool rightori){
        rightOri = rightori;
        if(rightori){
            right();
        }
        else{
            left();
        }
    }
    public void left(){
        toolTrans.parent = handBackground;
        toolTrans.localPosition = Vector2.zero;
        toolTrans.localRotation = Quaternion.identity;
        ForegroundArm.sprite = foregroundArm;
        BackgroundArm.sprite = backgroundArmHolding;
        ToolspriteRenderer.sortingOrder = orderInLayer2;

        focus = FocusBackground;
    }
    public void right(){
        toolTrans.parent = handForeground;
        toolTrans.localPosition = Vector2.zero;
        toolTrans.localRotation = Quaternion.identity;
        BackgroundArm.sprite = backgroundArm;
        ForegroundArm.sprite = foregroundArmHolding;
        ToolspriteRenderer.sortingOrder = OrderInLayer1;

        focus = FocusForeground;
    }
}
