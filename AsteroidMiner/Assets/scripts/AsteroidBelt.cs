using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class AsteroidBelt : MonoBehaviour
{
    public int seed;
    public float noiseScale;
    public Transform player;
    public float sectorSize =20f;
    public int asteroidGridSize = 6;
    public float jiggleRoom = 0.25f;
    public int loadDistance = 2;
    public float unloadDistance = 60f;
    public float itemUnload = 120f;
    items items;
    public Dictionary<Vector2Int,List<Object>> sectors = new Dictionary<Vector2Int, List<Object>>();
    public List<Vector2Int> sectorsToSave = new List<Vector2Int>();
    public List<Transform> itemsInWorld = new List<Transform>();
    
    public GameObject[] AsteroidObjects;
    public Vector2 sizeMultiplierRange;
    List<GameObject> loadedSectors = new List<GameObject>();
    [System.Serializable]
    public struct Object
    {
        public Object(Vector2 pos, int ty, float rot, float si){
            position = new float[2]{pos.x,pos.y};
            type = ty;
            rotation = rot;
            size = si;
        }
        public float[] position;
        public int type;
        public float rotation;
        public float size;
    }
    void Start()
    {
        items = items.instance;
        functionsToRunInMainThread = new List<Action>();
        if(seed == 0)
            seed = UnityEngine.Random.Range(-10000,10000);
        loadSectors();
    }

    public void load(SaveData data){
        SaveData.Objekte[] sects = data.sectors;
        foreach(SaveData.Objekte objs in sects){
            sectors.Add(new Vector2Int(objs.key[0],objs.key[1]),new List<Object>(objs.objs));
            sectorsToSave.Add(new Vector2Int(objs.key[0],objs.key[1]));
        }
        seed = data.seed;
    }

    // Update is called once per frame
    void Update()
    {
        RunFunctions();
        loadSectors();
    }
    Vector2Int previousPlayerSector;
    bool start = true;
    void loadSectors(){
        // if(player.position.magnitude < 10900)
        //     return;
        Vector2Int playerSector = new Vector2Int(Mathf.FloorToInt(player.position.x/sectorSize),Mathf.FloorToInt(player.position.y/sectorSize));
        if(start || playerSector != previousPlayerSector){
            start = false;
            
            List<Vector2> positions = new List<Vector2>();
            for(int i = loadedSectors.Count-1; i>=0; i--){
                if(Mathf.Abs(((Vector2)player.position-((Vector2)loadedSectors[i].transform.position)+Vector2.one*(sectorSize/2)).magnitude)>unloadDistance){
                    GameObject ls = loadedSectors[i];
                    loadedSectors.RemoveAt(i);
                    Destroy(ls);
                }
                else{
                    positions.Add(loadedSectors[i].transform.position/sectorSize);
                }
            }
            for(int y = loadDistance; y>=-loadDistance; y--){
                for(int x = -loadDistance; x<=loadDistance; x++){
                    Vector2Int sec = playerSector+new Vector2Int(x,y);
                    if(!(positions.Contains(sec))){
                        
                        loadSector(sec);
                    }
                }
            }
            dealWithItems();
        }

        previousPlayerSector = playerSector;
    }
    void dealWithItems(){
        foreach(Transform t  in itemsInWorld){
            if(Vector2.Distance(t.position,player.position) > itemUnload){
                t.gameObject.SetActive(false);
            }
            else{
                t.gameObject.SetActive(true);
            }
        }
    }
    //List<Vector2> currentlyConstructingLOL = new List<Vector2>(); //falls Sector geladen wird während anderer Thread ihn gerade generiert
    void loadSector(Vector2Int pos){
        if(sectors.ContainsKey(pos)){
            spawnAsteroids(pos);
        }
        else {
            //newSector(pos);
            StartThreadedFunction(() => {newSector(pos);});
        }

    }
    object lockDings = new object();
    public float m = 0.0005f;
    public float q = 0.98f;
    void newSector(Vector2Int pos){
        //seed+(int)pos.magnitude+(int)pos.x
        System.Random rand = new System.Random((int)(seed + 0.5f*(pos.x+pos.y)*(pos.x+pos.y+1)+pos.y));
        List<Object> astros = new List<Object>();
        float scale = sectorSize/asteroidGridSize;
        Vector2 sectorPos = (Vector2)pos*sectorSize;
        for(int x =0; x<asteroidGridSize; x++){
            for(int y =0; y<asteroidGridSize; y++){
                float xcord = (sectorPos.x+(float)x*scale);
                float ycord = (sectorPos.y+(float)y*scale);
                //float density = Mathf.Clamp((ycord-11000) / (maxDensityY-11000),0,0.9f);
                float dist = new Vector2(xcord,ycord).magnitude;
                float xdens = dist-11000;
                float density = (m*xdens +1-Mathf.Pow(q,xdens))/2;
                float perlin = Mathf.PerlinNoise(seed+xcord*noiseScale*0.9876f/sectorSize,seed+ycord*noiseScale*0.9876f/sectorSize);
                if((float)rand.NextDouble()<density){//perlin>(1-0.5f*density) || perlin < 0.4f*density){

                    Object ast = new Object(new Vector2(x*scale,y*scale)+new Vector2((float)rand.NextDouble()-0.5f,(float)rand.NextDouble()-0.5f)*2f*jiggleRoom,rand.Next(0,AsteroidObjects.Length),(float)rand.NextDouble()*360,(float)rand.NextDouble()*(sizeMultiplierRange.y-sizeMultiplierRange.x)+sizeMultiplierRange.x);
                    astros.Add(ast);
                }
            }
        }
        lock (lockDings)
        {
            if(!sectors.ContainsKey(pos))
                sectors.Add(pos,astros);
        }
        
        Action aFunction = () => {
            
            spawnAsteroids(pos);
        };
        QueueMainThreadFunction(aFunction);
    }
    void spawnAsteroids(Vector2Int pos){
        List<Object> objects = sectors[pos];
        GameObject sector = new GameObject();
        sector.transform.position = (Vector2)pos*sectorSize;
        for(int i = 0; i < objects.Count; i++){
            
            // if(objects[i].asteroid){
            GameObject[] list = AsteroidObjects;
            GameObject gm = GameObject.Instantiate(list[objects[i].type],(Vector2)sector.transform.position+new Vector2(objects[i].position[0],objects[i].position[1]),Quaternion.identity,sector.transform);
            // }
            // else{
            //     gm = items.DropItem(objects[i].type,(Vector2)sector.transform.position+new Vector2(objects[i].position[0],objects[i].position[1])).gameObject;
            //     gm.transform.parent = sector.transform;

            // }
            gm.GetComponent<Rigidbody2D>().rotation = objects[i].rotation;
            gm.transform.localScale *=objects[i].size; 
            
        }
        loadedSectors.Add(sector);
    }
    public void DeleteObject(Transform spaceObject){

        Vector2Int sector = Vector2Int.FloorToInt(spaceObject.parent.position/sectorSize);
        if(!sectors.ContainsKey(sector)){
           return;
        }
        List<Object> spaceObjects = sectors[sector];
        foreach(Object obj in spaceObjects){
            Vector2 pos = new Vector2(obj.position[0],obj.position[1]);
            if(pos == (Vector2)spaceObject.position-(Vector2)sector*sectorSize){
                sectors[sector].Remove(obj);
                if(!sectorsToSave.Contains(sector)){
                    sectorsToSave.Add(sector);
                }
                
                return;
            }
        }
    }
    public void DeleteObject(Transform spaceObject,int index){
        Vector2Int sector = Vector2Int.FloorToInt(spaceObject.parent.position/sectorSize);
        if(!sectors.ContainsKey(sector)){
           return;
        }
        List<Object> spaceObjects = sectors[sector];
        if(spaceObjects.Count < index+1)
            return;
        spaceObjects.RemoveAt(index);
        if(!sectorsToSave.Contains(sector)){
            sectorsToSave.Add(sector);
        }
        // foreach(Object obj in spaceObjects){
        //     Vector2 pos = new Vector2(obj.position[0],obj.position[1]);
        //     if(pos == (Vector2)spaceObject.position-(Vector2)sector*sectorSize){
        //         sectors[sector].Remove(obj);
        //         if(!sectorsToSave.Contains(sector)){
        //             sectorsToSave.Add(sector);
        //         }
        //         return;
        //     }
        // }
    }
    public void AddObject(Object obj, GameObject sector, bool spawn){
        Vector2Int sectorKey = Vector2Int.FloorToInt(sector.transform.position/sectorSize);
        if(sectors.ContainsKey(sectorKey)){
            sectors[sectorKey].Add(obj);
            if(!sectorsToSave.Contains(sectorKey)){
                sectorsToSave.Add(sectorKey);
            }
            if(spawn){
                GameObject[] list = AsteroidObjects;
                GameObject gm = GameObject.Instantiate(list[obj.type],(Vector2)sector.transform.position+new Vector2(obj.position[0],obj.position[1]),Quaternion.identity,sector.transform);
                
                gm.GetComponent<Rigidbody2D>().rotation = obj.rotation;
                gm.transform.localScale *=obj.size; 
            }
            
        }
            
    }
    public void AddItem(int item, Vector2 position, float rotation, float scale){
        Transform i = items.DropItem(item,position);
        i.eulerAngles = new Vector3(0,0,rotation);
        i.localScale = Vector3.one * scale;
        itemsInWorld.Add(i);
    }
    public void DeleteItem(Transform i){
        itemsInWorld.Remove(i);
        Destroy(i.gameObject);
    }

    //threading 
    List<Action> functionsToRunInMainThread;

    public void StartThreadedFunction(Action someFunction){
        Thread t = new Thread(new ThreadStart(someFunction));
        t.Start();
    }
    object LockDings2 = new object();
    public void QueueMainThreadFunction(Action someFunction){
        lock(LockDings2){
            functionsToRunInMainThread.Add(someFunction);
        }
        
    }
    void RunFunctions(){
        while(functionsToRunInMainThread.Count >0){
            lock(LockDings2){
                Action someFunc = functionsToRunInMainThread[0];
                functionsToRunInMainThread.RemoveAt(0);
                if(someFunc != null){
                    someFunc();
                }
            }
            
            
        }
        
    }
    
}

