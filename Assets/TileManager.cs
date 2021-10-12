using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /* tmp variable*/
    public Sprite floorSprite;

    public Vector2 origin = new Vector2(0, 0);

    private Dictionary<ulong, Tile> tiles = new Dictionary<ulong, Tile>();

    public static TileManager singleton { get; private set; }

    void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }
    }

    

    public void CreateTile(Vector2 position)
    {
        var index = GetIndexAt(position);
        CreateTile(index.x, index.y);
    }

    public void CreateTile(int x, int y)
    {
        ulong key = GetKey(x, y);

        if (tiles.ContainsKey(key))
        {
            string message = "Tried to create tile that already exists at position " + GetPosition(x, y).ToString() + ", index [" + x.ToString() + ", " + y.ToString() + "]";
            Debug.LogError(message);
            throw new Exception(message);
        }

        Tile tile = SpawnTile(x, y);
        tiles[key] = tile;


    }

    public void UpdateTile(int x, int y)
    {
        ulong key = GetKey(x, y);

        if (!tiles.ContainsKey(key))
        {
            string message = "Tried to update tile that doesn't exists at position " + GetPosition(x, y).ToString() + ", index [" + x.ToString() + ", " + y.ToString() + "]";
            Debug.LogError(message);
            throw new Exception(message);
        }
    }

    public void DestroyTile(int x, int y)
    {
        ulong key = GetKey(x, y);

        if (!tiles.ContainsKey(key))
        {
            string message = "Tried to destroy tile that doesn't exists at position " + GetPosition(x, y).ToString() + ", index [" + x.ToString() + ", " + y.ToString() + "]";
            Debug.LogError(message);
            throw new Exception(message);
        }

        var obj = tiles[key];
        tiles.Remove(key);

        Destroy(obj);
    }

    public Vector2Int GetIndexAt(Vector2 position)
    {
        return new Vector2Int((int)Math.Round(position.x - origin.x), (int)Math.Round(position.y - origin.y));
    }

    public Vector2 GetPosition(int x, int y)
    {
        return origin + new Vector2(x * 1.0f, y * 1.0f);
    }

    private ulong GetKey(int x, int y)
    {
        return (((ulong)y) << 32) + (ulong)x;
    }

    private Tile SpawnTile(int x, int y)
    {
        GameObject obj = new GameObject("[" + x.ToString() + "," + y.ToString() + "]", new[] { typeof(Tile) });
        obj.transform.parent = transform;
        obj.transform.position = GetPosition(x, y);
        obj.GetComponent<Tile>().SetSprite(floorSprite);
        obj.GetComponent<Tile>().start();
        return obj.GetComponent<Tile>();
    }

    /*tmp code*/
    public void Start()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                CreateTile(i, j);
    }

    
}
