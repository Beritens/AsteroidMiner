﻿using UnityEngine;
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
    public static void SavePlayer(SaveDataPlayer sD, string name){
        string path = Application.persistentDataPath+"/" + name +"Player.lol";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path+";-;", FileMode.Create);

        formatter.Serialize(stream,sD);
        stream.Close();
        if(File.Exists(path)){
            File.Delete(path);
        }
        File.Move(path+";-;",path);
    }
    public static SaveDataPlayer loadPlayer(string name){
        string path = Application.persistentDataPath+"/" + name +"Player.lol";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveDataPlayer sD = (SaveDataPlayer)formatter.Deserialize(stream);
            stream.Close();
            return sD;
        }
        else{
            return null;
        }
    }
    public static void SaveObjects(SaveObjectData sD, string name){
        string path = Application.persistentDataPath+"/" + name +"Objects.lol";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path+";-;", FileMode.Create);

        formatter.Serialize(stream,sD);
        stream.Close();
        if(File.Exists(path)){
            File.Delete(path);
        }
        File.Move(path+";-;",path);
    }
    public static SaveObjectData loadObjects(string name){
        string path = Application.persistentDataPath+"/" + name +"Objects.lol";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveObjectData sD = (SaveObjectData)formatter.Deserialize(stream);
            stream.Close();
            return sD;
        }
        else{
            return null;
        }
    }
}
