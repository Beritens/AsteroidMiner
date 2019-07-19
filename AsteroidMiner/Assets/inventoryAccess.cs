using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class inventoryAccess : window
{
    public Transform slotsContainer;
    public Transform toolsContainer;
    public Transform activeContainer;
    Vector2Int inHand;
    GameObject HandObj;
    public GameObject slotPre;
    inventory inv;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        inv = inventory.instance;
    }
    void Update()
    {
        if(Input.GetButtonDown("inventory") && !Open){
            open();
            UpdateInventory();
        }
        else if(Input.GetButtonDown("Cancel") && Open || Input.GetButtonDown("inventory") && Open){
            close();
        }
    }
    void UpdateInventory(){
        uSlots();
    }
    void uSlots(){
        Vector2Int[] slots = inv.GetSlots();
        for(int i = 0; i<slots.Length; i++){
            SetItem(slots[i],slotsContainer.GetChild(i));
        }
    }
    void SetItem(Vector2Int slot,Transform obj){
        if(slot.y == 0){
            obj.GetChild(0).GetComponent<Image>().color = new Color(0,0,0,0);
            obj.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        else{
            Image im = obj.GetChild(0).GetComponent<Image>();
            im.sprite = items.instance.itemObjects[slot.x].sprite;
            im.color = Color.white;
            obj.GetChild(1).GetComponent<TextMeshProUGUI>().text = (slot.y >1)?slot.y.ToString(): "";
        }
    }
    public void Drag(int slot, Transform con){
        if(con == slotsContainer){
            Vector2Int sl = inv.GetSlot(slot);
            createDrag(sl, slot);
            inv.RemoveFromSlots(slot);
            SetItem(inv.GetSlot(slot),slotsContainer.GetChild(slot));
            
        }
        else if(con == toolsContainer){
            Vector2Int sl = inv.GetTool(slot);
            createDrag(sl, slot);
            inv.RemoveFromTools(slot);
            SetItem(inv.GetTool(slot),toolsContainer.GetChild(slot));
            
        }
    }
    void createDrag(Vector2Int sl, int slot){
        if(sl.y != 0){
            HandObj = GameObject.Instantiate(slotPre,Input.mousePosition,Quaternion.identity,Window.transform);
            HandObj.GetComponent<slot>().onHand = true;
            HandObj.GetComponent<Image>().enabled = false;
            Image im = HandObj.transform.GetChild(0).GetComponent<Image>();
            im.sprite = items.instance.itemObjects[sl.x].sprite;
            im.color = Color.white;
            HandObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (sl.y >1)?sl.y.ToString(): "";
            inHand = sl;
            
        }
    }
    public void releaseOnHand(){
        PointerEventData pointerData = new PointerEventData (EventSystem.current)
         {
             pointerId = -1,
         };
         
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        foreach(RaycastResult r in results){
            slot s = r.gameObject.GetComponent<slot>();
            if(s != null && s.onHand == false){
                int i = s.gameObject.transform.GetSiblingIndex();
                int re = 0 ;//inv.AddToSlotsAmount(inHand.x,inHand.y,i);
                Transform p = r.gameObject.transform.parent;
                int o = 0;
                if(p == slotsContainer){
                    re = inv.AddToSlotsAmount(inHand.x,inHand.y,i,out o);
                }
                else if(p == toolsContainer){
                    re = inv.AddToToolsAmount(inHand.x,inHand.y,i,out o);
                }
                if(re == 0){
                    Destroy(HandObj);
                    inHand = Vector2Int.zero;
                    if(p == slotsContainer){
                        SetItem(inv.GetSlot(i),slotsContainer.GetChild(i));
                    }
                    else if(p == toolsContainer){
                        SetItem(inv.GetTool(i),toolsContainer.GetChild(i));
                     }
                    
                }
                else if(re == inHand.y){
                    inv.AddToSlotsAmount(inHand.x,inHand.y,true);
                    Destroy(HandObj);
                    inHand = Vector2Int.zero;
                    uSlots();
                }
                return;
                
            }
        }
        inv.AddToSlotsAmount(inHand.x,inHand.y,true);
        Destroy(HandObj);
        inHand = Vector2Int.zero;
        uSlots();
    }
}
