using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ressources : MonoBehaviour
{
    public float maxStorage;
    float storage;
    float[] ressourcesCount = new float[1];
    public float[] storageMultiplier;
    public GameObject[] panels;
    public TextMeshProUGUI[] ressourcesNumbers;

    public void Add(int type){
        ressourcesCount[type]++;
        storage += storageMultiplier[type];
        ressourcesNumbers[type].text = ressourcesCount[type].ToString();
        if(ressourcesCount[type] == 1){
            panels[type].SetActive(true);
        }
    }
    public void reload(float[] res){
        ressourcesCount = res;
        storage = 0;
        for (int i = 0; i < res.Length; i++)
        {
            storage += res[i]*storageMultiplier[i];
            if(res[i]!= 0){
                panels[i].SetActive(true);
                ressourcesNumbers[i].text = ressourcesCount[i].ToString();
            }
        }
    }
    public float GetStorage(){
        return storage;
    }
    public float[] GetRessources(){
        return ressourcesCount;
    }
}
