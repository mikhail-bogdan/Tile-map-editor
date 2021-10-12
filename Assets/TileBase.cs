using System.Collections;
using UnityEngine;

public class TileBase : ScriptableObject
{
    // Should be unique
    public string id;

    public string type;

    public GameObject prefab;
}