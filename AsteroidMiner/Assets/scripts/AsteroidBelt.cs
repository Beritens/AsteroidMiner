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
    public Dictionary<Vector2Int,List<Object>> sectors = new Dictionary<Vector2Int, List<Object>>();
    public List<Vector2Int> sectorsToSave = new List<Vector2Int>();
    
    public GameObject[] AsteroidObjects;
    public GameObject[] Objects;
    public Vector2 sizeMultiplierRange;
    List<GameObject> loadedSectors = new List<GameObject>();
    [System.Serializable]
    public struct Object
    {
        public Object(Vector2 pos, int ty, float rot, float si,bool ast){
            position = new float[2]{pos.x,pos.y};
            type = ty;
            rotation = rot;
            size = si;
            asteroid = ast;
        }
        public float[] position;
        public int type;
        public float rotation;
        public float size;
        public bool asteroid;
    }
    void Start()
    {
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
        }

        previousPlayerSector = playerSector;
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
    void newSector(Vector2Int pos){
        System.Random rand = new System.Random(seed+(int)pos.magnitude+(int)pos.x);
        List<Object> astros = new List<Object>();
        float scale = sectorSize/asteroidGridSize;
        Vector2 sectorPos = (Vector2)pos*sectorSize;
        for(int x =0; x<asteroidGridSize; x++){
            for(int y =0; y<asteroidGridSize; y++){
                float xcord = seed+(sectorPos.x+(float)x*scale)/sectorSize*noiseScale*0.9876f;
                float ycord = seed+(sectorPos.y+(float)y*scale)/sectorSize*noiseScale*0.9876f;
                float perlin = Mathf.PerlinNoise(xcord,ycord);
                if(perlin>0.5f || perlin < 0.2f){

                    Object ast = new Object(new Vector2(x*scale,y*scale)+new Vector2((float)rand.NextDouble()-0.5f,(float)rand.NextDouble()-0.5f)*2f*jiggleRoom,rand.Next(0,AsteroidObjects.Length),(float)rand.NextDouble()*360,(float)rand.NextDouble()*(sizeMultiplierRange.y-sizeMultiplierRange.x)+sizeMultiplierRange.x,true);
                    astros.Add(ast);
                }
            }
        }
        Debug.Log(astros);
        lock (lockDings)
        {
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
            GameObject[] list = objects[i].asteroid? AsteroidObjects: Objects;
            GameObject gm = GameObject.Instantiate(list[objects[i].type],(Vector2)sector.transform.position+new Vector2(objects[i].position[0],objects[i].position[1]),Quaternion.identity,sector.transform);
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
    public void AddObject(Object obj, GameObject sector, bool spawn){
        Vector2Int sectorKey = Vector2Int.FloorToInt(sector.transform.position/sectorSize);
        if(sectors.ContainsKey(sectorKey)){
            sectors[sectorKey].Add(obj);
            if(!sectorsToSave.Contains(sectorKey)){
                sectorsToSave.Add(sectorKey);
            }
            if(spawn){
                GameObject[] list = obj.asteroid? AsteroidObjects: Objects;
                GameObject gm = GameObject.Instantiate(list[obj.type],(Vector2)sector.transform.position+new Vector2(obj.position[0],obj.position[1]),Quaternion.identity,sector.transform);
                gm.GetComponent<Rigidbody2D>().rotation = obj.rotation;
                gm.transform.localScale *=obj.size; 
            }
            
        }
            
    }

    //threading 
    List<Action> functionsToRunInMainThread;
    public void StartThreadedFunction(Action someFunction){
        Thread t = new Thread(new ThreadStart(someFunction));
        t.Start();
    }
    public void QueueMainThreadFunction(Action someFunction){
        
        functionsToRunInMainThread.Add(someFunction);
    }
    void RunFunctions(){
        while(functionsToRunInMainThread.Count >0){
            Action someFunc = functionsToRunInMainThread[0];
            functionsToRunInMainThread.RemoveAt(0);
            if(someFunc != null){
                someFunc();
            }
            
            
        }
    }
    
}

