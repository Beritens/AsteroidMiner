using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class inventory : MonoBehaviour
{
    public static inventory instance;
    float money = 20f;
    Vector2Int[] slots = new Vector2Int[25];
    Vector2Int[] tools = new Vector2Int[4];
    Vector2Int[] active = new Vector2Int[6];
    void Awake()
    {
        instance = this;
    }
    public void reload(int[] slotItems, int[] slotCounts, int[] toolItems, int[] toolCounts){
        for(int i = 0; i< slots.Length; i++){
            slots[i] = new Vector2Int(slotItems[i],slotCounts[i]);
        }
        for(int i = 0; i< tools.Length; i++){
            tools[i] = new Vector2Int(toolItems[i],toolCounts[i]);
        }
        // ressourcesCount = res;
        // storage = 0;
        // for (int i = 0; i < res.Length; i++)
        // {
        //     storage += res[i]*storageMultiplier[i];
        //     if(res[i]!= 0){
        //         panels[i].SetActive(true);
        //         ressourcesNumbers[i].text = ressourcesCount[i].ToString();
        //     }
        // }
    }
    #region
    public Vector2Int[] GetSlots(){
        return slots;
    }
    public Vector2Int[] GetTools(){
        return tools;
    }
    public Vector2Int[] GetActives(){
        return active;
    }
    public Vector2Int GetSlot(int i){
        return slots[i];
    }
    public Vector2Int GetTool(int i){
        return tools[i];
    }
    public Vector2Int GetActive(int i){
        return active[i];
    }
     #endregion
    
    #region //slots
    public bool AddToSlots(int item){
        for(int i = 0; i< slots.Length; i++){
            if(slots[i].x == item && slots[i].y <100 && items.instance.itemObjects[item].stackable){
                slots[i].y++;
                return true;
            }
        }
        for(int i = 0; i< slots.Length; i++){
            if (slots[i].y ==0){
                slots[i].x = item;
                slots[i].y = 1;
                return true;
            }
        }
        
        return false;

    }
    public int AddToSlotsAmount(int item, int number,bool repeat){
        for(int i = 0; i< slots.Length; i++){
            if(slots[i].x == item && slots[i].y <100 && items.instance.itemObjects[item].stackable){
                if(100 -slots[i].y >= number){
                    slots[i].y+= number;
                    return 0;
                }
                else{
                    int rest = slots[i].y+number -100;
                    slots[i].y =  100;
                    if(repeat){
                        while(rest >0){
                            int r = AddToSlotsAmount(item,rest,false);
                            if(r== rest){
                                return rest;
                            }
                            rest = r;

                        }
                        return 0;

                    }
                    else{
                        return rest;
                    }
                }
            }
        }
        for(int i = 0; i< slots.Length; i++){
            if (slots[i].y ==0){
                slots[i].x = item;
                slots[i].y += number;
                return 0;
            }
        }
        
        return number;

    }
    public int AddToSlotsAmount(int item, int number, int slot, out int reject){
        if(slots[slot].y == 100 || slots[slot].x != item &&slots[slot].y >0 || !items.instance.itemObjects[item].stackable &&slots[slot].y >0){
            reject = 0;
            return number;
        }
        else if(100-slots[slot].y>= number){
            slots[slot].x = item;
            slots[slot].y += number;
            reject = 0;
            return 0;
        }  
        else{
            int rest = slots[slot].y+number-100;
            slots[slot].x = item;
            slots[slot].y = 100;
            reject = AddToSlotsAmount(item, rest, true);
            
            return rest;
        }
    }
     public bool AddToSlots(int item, int slot){
        if(slots[slot].x == item && slots[slot].y <100 && items.instance.itemObjects[item].stackable){
            slots[slot].y++;
            return true;
        }
        else if (slots[slot].y ==0){
            slots[slot].x = item;
            slots[slot].y = 1;
            return true;
        }
        return false;

    }
    public void RemoveFromSlots(int slot){
        slots[slot] = Vector2Int.zero;
    }
    #endregion
    public bool AddToTools(int item, int slot){
        if (tools[slot].y ==0){
            tools[slot].x = item;
            tools[slot].y = 1;
            return true;
        }
        return false;
    }
    public int AddToToolsAmount(int item, int number, int slot, out int reject){
    if(tools[slot].y == 100 || (tools[slot].x != item &&tools[slot].y >0) || (!items.instance.itemObjects[item].stackable &&tools[slot].y >0) || (items.instance.itemObjects[item].type != 1)){
        Debug.Log((tools[slot].y == 100) + " "+ (tools[slot].x != item &&tools[slot].y >0)+ " " +(!items.instance.itemObjects[item].stackable &&tools[slot].y >0) +" "+ (items.instance.itemObjects[item].type != 1));
        reject = 0;
        return number;
    }
    else if(100-tools[slot].y>= number){
        tools[slot].x  = item;
        tools[slot].y += number;
        reject = 0;
        return 0;
    }  
    else{
        //tools[slot].x  = item;
        
        int rest = tools[slot].y+number-100;
        tools[slot].y  = 100;
        reject = AddToSlotsAmount(item, rest, true);
        return rest;
    }
    }
    public void RemoveFromTools(int slot){
        tools[slot] = Vector2Int.zero;
    }
    public bool AddToActive(int item, int slot){
        if (active[slot].y ==0){
            active[slot].x = item;
            active[slot].y = 1;
            return true;
        }
        return false;
    }
    public void RemoveFromActive(int slot){
        active[slot] = Vector2Int.zero;
    }
    public bool AddMoney(float mon){
        if(money + mon < 0)
            return false;
        money += mon;
        return true;
    }
}
