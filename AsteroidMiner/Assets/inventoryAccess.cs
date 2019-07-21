using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class inventoryAccess : window
{
    public toolManager toolManager;
    public Transform slotsContainer;
    public Transform toolsContainer;
    public Transform activeContainer;
    public Transform toolsDisplay;
    public Color toolDisplayColor;
    Vector2Int inHand;
    GameObject HandObj;
    public GameObject slotPre;
    inventory inv;
    int selectedToolSlot = 0;
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        inv = inventory.instance;
        SelectTool(true);
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
        if(Open){
            if(inHand.y != 0){
                if(Input.GetButtonDown("Fire2")){
                    inHand.y--;
                    PlaceItem(new Vector2Int(inHand.x,1),getMouseSlot());
                    UpdateHand();
                }
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0){
            SelectTool(false);
            selectedToolSlot= (int)Mathf.Repeat(selectedToolSlot-(int)(10*Input.GetAxis("Mouse ScrollWheel")),4);
            SelectTool(true);
        }
    }
    void SelectTool(bool select){
        toolsDisplay.GetChild(selectedToolSlot).GetComponent<Image>().color = select? toolDisplayColor: new Color(0,0,0,0);
        Vector2Int item = inv.GetTool(selectedToolSlot);
        if(item.y == 0){
            toolManager.deselectTool();
            return;
        }
        if(select){
            toolManager.selectTool(item.x);
        }
        else{
            toolManager.deselectTool();
        }
    }
    void UpdateInventory(){
        uSlots();
        uTools();
    }
    void uSlots(){
        Vector2Int[] slots = inv.GetSlots();
        for(int i = 0; i<slots.Length; i++){
            SetItem(slots[i],slotsContainer.GetChild(i));
        }
    }
    void uTools(){
        Vector2Int[] tools = inv.GetTools();
        for(int i = 0; i<tools.Length; i++){
            SetItem(tools[i],toolsContainer.GetChild(i));
            SetItem(tools[i],toolsDisplay.GetChild(i));
        }
        SelectTool(false);
        SelectTool(true);
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
        Transform sloty = getMouseSlot();
        if(sloty != null){
            PlaceItem(inHand,sloty);
            inHand.y = 0;
            UpdateHand();  
                
            
        }
        else{
            inv.AddToSlotsAmount(inHand.x,inHand.y,true);
            inHand.y = 0;
            UpdateHand();
            uSlots();
        }
        
    }
    void UpdateHand(){
        if(inHand.y == 0){
            inHand = Vector2Int.zero;
            Destroy(HandObj);
        }
        else{
            HandObj.GetComponentInChildren<TextMeshProUGUI>().text = inHand.y.ToString();
        }
    }
    private void PlaceItem(Vector2Int item, Transform slot){
        int i = slot.GetSiblingIndex();
        int re = item.y ;//inv.AddToSlotsAmount(inHand.x,inHand.y,i);
        Transform p = slot.parent;
        int o = 0;
        if(p == slotsContainer){
            re = inv.AddToSlotsAmount(item.x,item.y,i,out o);
        }
        else if(p == toolsContainer){
            re = inv.AddToToolsAmount(item.x,item.y,i,out o);
        }
        if(re != item.y){
            if(re >0){
                uSlots();
            }
            else{
                if(p == slotsContainer){
                    SetItem(inv.GetSlot(i),slotsContainer.GetChild(i));
                }
                else if(p == toolsContainer){
                    uTools();
                }
            }
            
            
        }
        else{
            inv.AddToSlotsAmount(item.x,item.y,true);
            uSlots();
        }
    }
    private Transform getMouseSlot(){
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
                return s.transform;
            }
        }
        return null;
    }
}
