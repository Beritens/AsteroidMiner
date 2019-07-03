using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveWorld(SaveData sD, string name){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath+"/" + name +".lol";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream,sD);
        stream.Close();
    }
    public static SaveData loadWorld(string name){
        string path = Application.persistentDataPath+"/" + name +".lol";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData sD = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return sD;
        }
        else{
            return null;
        }
    }
}
