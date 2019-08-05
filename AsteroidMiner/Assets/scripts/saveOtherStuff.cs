using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveOtherStuff : MonoBehaviour
{
    public static saveOtherStuff instance;
    public List<Transform> transforms = new List<Transform>();
    public save saveCom;
    // Start is called before the first frame update

    // Update is called once per frame
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }
    public void save(){
        saveTransform[] saveTransforms = new saveTransform[transforms.Count];
        for (int i = 0; i < transforms.Count; i++)
        {
            saveTransforms[i] = new saveTransform(transforms[i].position,transforms[i].eulerAngles.z,new Vector2(transforms[i].localScale.x,transforms[i].localScale.y),transforms[i].GetComponent<WorldObject>().item);
        }
        SaveObjectData sOD = new SaveObjectData(saveTransforms);
        SaveSystem.SaveObjects(sOD,saveCom.saveName);
    }
    public void load(){
        instance = this;
        items items = items.instance;
        SaveObjectData sOD = SaveSystem.loadObjects(saveCom.saveName);
        if(sOD == null)
            return;
        for (int i = 0; i < sOD.transforms.Length; i++)
        {
            
            Transform t = items.spawnObject(sOD.transforms[i].item,new Vector2(sOD.transforms[i].position[0],sOD.transforms[i].position[1]));
            t.eulerAngles = new Vector3(0,0,sOD.transforms[i].rotation);
            t.localScale = new Vector3(sOD.transforms[i].scale[0],sOD.transforms[i].scale[1],1);
            transforms.Add(t);
        }
    }
    public void Add(Transform transform){
        transforms.Add(transform);

    }
}
[System.Serializable]
public class SaveObjectData{
    public SaveObjectData(saveTransform[] sT){
        this.transforms = sT;
    }
    public saveTransform[] transforms;
}
