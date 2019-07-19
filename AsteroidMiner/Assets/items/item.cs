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
    public float[] values;
    public bool stackable;
    [Tooltip("0 = normal, 1 = tool, 2 = active")]
    public int type = 0;
}
