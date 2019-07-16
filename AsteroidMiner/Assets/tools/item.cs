using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "item")]
public class item : ScriptableObject
{
    public new string name;
    public float cost;
    public float resellValue;
    public Sprite sprite;
    public GameObject prefab;
}
