using System.Collections;
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
    public saveOtherStuff saveOtherStuff;
    
    void Awake()
    {
        Instance = this;
        if(saveName != null){
            loadWorld();
            saveOtherStuff.load();
            savePlayer.load();
        }
    }
    Thread t;
    void saveWorld(){
        if(t!= null && t.ThreadState == ThreadState.Running){
            t.Abort();
        }
        List<Transform> itemsInWorld = asteroidBelt.itemsInWorld;
        saveTransform[] it = new saveTransform[itemsInWorld.Count];
        for (int i = 0; i < itemsInWorld.Count; i++)
        {
            it[i] = new saveTransform(itemsInWorld[i].position,itemsInWorld[i].eulerAngles.z,new Vector2(itemsInWorld[i].localScale.x,itemsInWorld[i].localScale.y),itemsInWorld[i].GetComponent<ressource>().item);
        }
        string path =  Application.persistentDataPath+"/" + saveName +".lol";
        t = new Thread(() =>{
            SaveData.Objekte[] sectorArr = new SaveData.Objekte[asteroidBelt.sectorsToSave.Count];
            for(int i = 0; i< sectorArr.Length; i++){
                sectorArr[i] = new SaveData.Objekte(asteroidBelt.sectors[asteroidBelt.sectorsToSave[i]],asteroidBelt.sectorsToSave[i]);
            }
            SaveData save = new SaveData(sectorArr,asteroidBelt.seed,it);
            SaveSystem.SaveWorld(save, path);
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
            saveOtherStuff.save();
            savePlayer.save();
        }
    }
    
}
[System.Serializable]
public class SaveData {
    public SaveData(Objekte[] sects, int seed, saveTransform[] itemsInWorld ){
        this.sectors = sects;
        this.seed = seed;
        this.items = itemsInWorld;//
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
    public saveTransform[] items;
}
[System.Serializable]
public class saveTransform{
    public saveTransform(Vector2 position, float rotation, Vector2 scale, int item){
        this.rotation = rotation;
        this.position = new float[2]{position.x,position.y};
        this.scale = new float[2]{scale.x,scale.y};
        this.item = item;
    }
    public int item;
    public float rotation;
    public float[] position;
    public float[] scale;
}
