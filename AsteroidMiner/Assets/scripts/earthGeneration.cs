using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class earthGeneration : MonoBehaviour
{
    public float radius;
    public GameObject tilePrefab;
    public Sprite[] tiles;
    public Sprite[] emptytiles;
    public float tileLength;
    public Transform player;
    float degreePerTile;
    public int genDist=4;
    public float unloadDist = 600f; 
    List<GameObject> loaded = new List<GameObject>(); 
    List<int> loadedT = new List<int>(); 
    public List<int> stuffOnTile = new List<int>();
    int seed;
    float tileNumber;

    // Start is called before the first frame update
    void Start()
    {
        float u = 2*Mathf.PI*radius;
        tileNumber = u/tileLength;
        degreePerTile = 360/tileNumber;
    }
    int prevTile = 69420;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.position.magnitude > radius + 100f)
            return;
        float playerDegrees = Mathf.Atan2(player.position.x,player.position.y)*Mathf.Rad2Deg ;
        int playerTile = Mathf.FloorToInt(playerDegrees/degreePerTile);
        if(playerTile != prevTile){
            Gen(playerTile);
        }
        prevTile = playerTile;
    }
    void Gen(int tile){
        for(int i = loadedT.Count-1;i >=  0;i--){
            if(loadedT[i]>tile+genDist || loadedT[i]<tile+genDist){
                GameObject g = loaded[i];
                loaded.RemoveAt(i);
                loadedT.RemoveAt(i);
                Destroy(g);
            }
        }
        for(int i=-genDist+1;i<genDist+1;i++){
            if(!loadedT.Contains(tile+i))
                createLand(tile+i);
        }
        
    }
    void createLand(int tile){
        System.Random rand = new System.Random(seed);
        int i = 0;
        float t = (int)(Mathf.Repeat(tile+tileNumber/2,tileNumber));
        while(i<t){
            i ++;
            rand.Next();
        }
        
        float angle = tile*degreePerTile;
        float air = angle* Mathf.Deg2Rad;
        Vector2 position = new Vector2(Mathf.Sin(air)*radius,Mathf.Cos(air)*radius);
        GameObject g = GameObject.Instantiate(tilePrefab,position,Quaternion.Euler(0,0,-(angle)));//+degreePerTile/2
        SpriteRenderer sR = g.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(!stuffOnTile.Contains(tile)){
            sR.sprite = tiles[rand.Next(0,tiles.Length)];
        }
        else{
            sR.sprite = emptytiles[rand.Next(0,emptytiles.Length)];
        }
        
        loaded.Add(g);
        loadedT.Add(tile);

    }
}
