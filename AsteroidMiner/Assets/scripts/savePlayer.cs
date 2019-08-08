using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savePlayer : MonoBehaviour
{
    public theBeginning tB;
    public Rigidbody2D player;
    public inventory inventory;
    public save saveCom;
    public saveOtherStuff saveOtherStuff;
    public void save(){
        int parent = -1;
        if(player.transform != null){
            parent = saveOtherStuff.transforms.IndexOf(player.transform.parent);
        }
        
        SaveDataPlayer sDP = new SaveDataPlayer(inventory.GetSlots(),inventory.GetTools(),inventory.GetActives(),player.position,player.velocity,player.rotation,parent, inventory.money);
        SaveSystem.SavePlayer(sDP,saveCom.saveName);
    }
    public void load(){
        
        SaveDataPlayer sDP = SaveSystem.loadPlayer(saveCom.saveName);
        if(sDP != null){
            inventory.reload(sDP.slotItems,sDP.slotCounts, sDP.toolItems,sDP.toolCounts,sDP.activeItems,sDP.activeCounts,sDP.money);
            player.GetComponent<reloadPlayer>().reload(sDP,saveOtherStuff.transforms);
        }
        else{
            tB.start();
        }
        
        
        
    }
}
[System.Serializable]
public class SaveDataPlayer{
    public SaveDataPlayer(Vector2Int[] slots,Vector2Int[] tools ,Vector2Int[] active,Vector2 position, Vector2 velocity, float rotation, int parent, float money){
        slotItems = new int[slots.Length];
        slotCounts = new int[slots.Length];
        for(int i = 0; i<slots.Length;i++){
            slotItems[i] = slots[i].x;
            slotCounts[i] = slots[i].y;
        }
        toolItems = new int[tools.Length];
        toolCounts = new int[tools.Length];
        for(int i = 0; i<tools.Length;i++){
            toolItems[i] = tools[i].x;
            toolCounts[i] = tools[i].y;
        }
        activeItems = new int[active.Length];
        activeCounts = new int[active.Length];
        for(int i = 0; i<tools.Length;i++){
            activeItems[i] = active[i].x;
            activeCounts[i] = active[i].y;
        }
        this.position = new float[2]{position.x,position.y};
        this.velocity = new float[2]{velocity.x,velocity.y};
        this.rotaion = rotation;
        this.money = money;
        this.parent = parent;
    }
    public int[] slotItems;
    public int[] slotCounts;

    public int[] toolItems;
    public int[] toolCounts;
    public int[] activeItems;
    public int[] activeCounts;
    //public Vector2Int actives;
    public float[] position;
    public float[] velocity;
    public float rotaion;
    public float money;
    public int parent;
}
