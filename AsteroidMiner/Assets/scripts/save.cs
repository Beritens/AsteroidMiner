﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class save : MonoBehaviour
{
    public string saveName;
    public AsteroidBelt asteroidBelt;
    public static save Instance;
    SaveData data;
    public savePlayer savePlayer;
    
    void Awake()
    {
        Instance = this;
        if(saveName != null){
            loadWorld();
            savePlayer.load();
        }
    }
    Thread t;
    void saveWorld(){
        string path = Application.persistentDataPath+"/" + saveName +".lol";
        if(t!= null && t.ThreadState == ThreadState.Running){
            t.Abort();
        }
        t = new Thread(() =>{
            SaveData.Objekte[] sectorArr = new SaveData.Objekte[asteroidBelt.sectorsToSave.Count];
            for(int i = 0; i< sectorArr.Length; i++){
                sectorArr[i] = new SaveData.Objekte(asteroidBelt.sectors[asteroidBelt.sectorsToSave[i]],asteroidBelt.sectorsToSave[i]);
            }
            SaveData save = new SaveData(sectorArr,asteroidBelt.seed);
            SaveSystem.SaveWorld(save,path);
            Debug.Log("saved");});
        t.Start();
        
    }
    void loadWorld(){
        data = SaveSystem.loadWorld(saveName);
        if(data != null){
            asteroidBelt.load(data);
        }
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J)){
            saveWorld();
            savePlayer.save();
        }
    }
    
}
[System.Serializable]
public class SaveData {
    public SaveData(Objekte[] sects, int seed){
        this.sectors = sects;
        this.seed = seed;
    }
    public int seed;
    [System.Serializable]
    public class Objekte
    {
        public int[] key;
        public Objekte(List<AsteroidBelt.Object> objList, Vector2Int key){
            this.objs = objList.ToArray();
            this.key = new int[2] {key.x,key.y};
        }
        public AsteroidBelt.Object[] objs;
    }
    public Objekte[] sectors;
}
