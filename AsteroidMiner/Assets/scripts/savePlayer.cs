using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savePlayer : MonoBehaviour
{
    public Rigidbody2D player;
    public inventory inventory;
    public save saveCom;
    public void save(){
        string path = Application.persistentDataPath+"/" + saveCom.saveName +"Player.lol";
        SaveDataPlayer sDP = new SaveDataPlayer(new float[1]{1},player.position,player.velocity,player.rotation);
        SaveSystem.SavePlayer(sDP,path);
    }
    public void load(){
        
        SaveDataPlayer sDP = SaveSystem.loadPlayer(saveCom.saveName);
        if(sDP == null)
            return;
        
        inventory.reload(sDP.storage);
        player.GetComponent<reloadPlayer>().reload(sDP);
        
        
    }
}
[System.Serializable]
public class SaveDataPlayer{
    public SaveDataPlayer(float[] storage, Vector2 position, Vector2 velocity, float rotation){
        this.storage = storage;
        this.position = new float[2]{position.x,position.y};
        this.velocity = new float[2]{velocity.x,velocity.y};
        this.rotaion = rotation;
    }
    public float[] storage;
    public float[] position;
    public float[] velocity;
    public float rotaion;
}