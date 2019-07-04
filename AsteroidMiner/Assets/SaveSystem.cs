using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveWorld(SaveData sD, string path){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path+";-;", FileMode.Create);

        formatter.Serialize(stream,sD);
        stream.Close();
        if(File.Exists(path)){
            File.Delete(path);
        }
        File.Move(path+";-;",path);
    }
    public static SaveData loadWorld(string name){
        string path = Application.persistentDataPath+"/" + name +".lol";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData sD = (SaveData)formatter.Deserialize(stream);
            stream.Close();
            return sD;
        }
        else{
            return null;
        }
    }
}
