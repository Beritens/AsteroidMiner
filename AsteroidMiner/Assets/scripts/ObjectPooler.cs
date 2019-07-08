using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class pool{
        public string tag;
        public GameObject prefab;
        public int size;
        
    }

    public static ObjectPooler Instance;
    void Awake()
    {
        Instance = this;
    }

    public List<pool> pools;
    public Dictionary<string,Queue<GameObject>> poolDictionary= new Dictionary<string,Queue<GameObject>>(); 
    void Start()
    {
        foreach(pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i<pool.size; i++){
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag,objectPool);
        }
    }
    public GameObject SpawnFromPool(string tag, Vector2 pos, float rotation){
        if(!poolDictionary.ContainsKey(tag)){
            Debug.Log("gib's net ;-;");
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = pos;
        objToSpawn.transform.eulerAngles = Vector3.forward*rotation;
        return objToSpawn;

    }
    public void EnqueueObj(string tag,GameObject obj){
        if(!poolDictionary.ContainsKey(tag)){
            Debug.Log("gib's net ;-;");
            return;
        }
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}
