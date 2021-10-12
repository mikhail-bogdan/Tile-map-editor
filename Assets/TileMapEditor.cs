using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileMapEditor : EditorWindow
{
    /* tmp variable*/
    public Sprite floorSprite;


    private TileManager tileManager;



    private bool enablePlacement = false;
    private bool enableDestroy = false;
    private Tile currentTile;

    private List<TileBase> assetList = new List<TileBase>();
    private List<GUIContent> assetIcons = new List<GUIContent>();
    private Vector2 scrollPositionSelection;
    private string searchString = "";
    private int assetIndex;

    [MenuItem("Game Engine Instruments/Tile Map Editor")]
    public static void ShowWindow()
    {
        string icon = "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAABN2lDQ1BBZG9iZSBSR0IgKDE5OTgpAAAokZWPv0rDUBSHvxtFxaFWCOLgcCdRUGzVwYxJW4ogWKtDkq1JQ5ViEm6uf/oQjm4dXNx9AidHwUHxCXwDxamDQ4QMBYvf9J3fORzOAaNi152GUYbzWKt205Gu58vZF2aYAoBOmKV2q3UAECdxxBjf7wiA10277jTG+38yH6ZKAyNguxtlIYgK0L/SqQYxBMygn2oQD4CpTto1EE9AqZf7G1AKcv8ASsr1fBBfgNlzPR+MOcAMcl8BTB1da4Bakg7UWe9Uy6plWdLuJkEkjweZjs4zuR+HiUoT1dFRF8jvA2AxH2w3HblWtay99X/+PRHX82Vun0cIQCw9F1lBeKEuf1UYO5PrYsdwGQ7vYXpUZLs3cLcBC7dFtlqF8hY8Dn8AwMZP/fNTP8gAAAAJcEhZcwAACxMAAAsTAQCanBgAAAW6aVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA2LjAtYzAwMiA3OS4xNjQ0NjAsIDIwMjAvMDUvMTItMTY6MDQ6MTcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCAyMS4yIChXaW5kb3dzKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjEtMDMtMTBUMjM6MDY6MDMrMDM6MDAiIHhtcDpNZXRhZGF0YURhdGU9IjIwMjEtMDMtMTBUMjM6MTA6MTgrMDM6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAzLTEwVDIzOjEwOjE4KzAzOjAwIiBkYzpmb3JtYXQ9ImltYWdlL3BuZyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo0ZWJlZGE0Zi1mNGI2LWE0NDYtYmU3ZC1mNTFjMzFlZGRlODciIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6ZjkyODQ0ZDAtZWNkYy1jMDQ3LWIzZjktMjQzZmRkNjE3MDYxIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZjkyODQ0ZDAtZWNkYy1jMDQ3LWIzZjktMjQzZmRkNjE3MDYxIiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iY3JlYXRlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDpmOTI4NDRkMC1lY2RjLWMwNDctYjNmOS0yNDNmZGQ2MTcwNjEiIHN0RXZ0OndoZW49IjIwMjEtMDMtMTBUMjM6MDY6MDMrMDM6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCAyMS4yIChXaW5kb3dzKSIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6NGViZWRhNGYtZjRiNi1hNDQ2LWJlN2QtZjUxYzMxZWRkZTg3IiBzdEV2dDp3aGVuPSIyMDIxLTAzLTEwVDIzOjEwOjE4KzAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjEuMiAoV2luZG93cykiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+rTKV/QAAALxJREFUeJzt2zEOwyAUwND+qve/MrlBGKrkDdgrDJbFgD5i1lqfk/lqAU0BtICmAFpAUwAtoPntNszMoxeFF+4hc7d4/AkogBbQFEALaAqgBTQF0AKaAmgBTQG0gKYAWkBTAC2gKYAW0BRAC2gKoAU023eBp+f2M7dj+7/Z+R9/AgqgBTQF0AKaAmgBTQG0gKYAWkBTAC2gKYAW0BRAC2gKoAU0BdACmgJoAc30b/BwCqAFNAXQApoCaAHNBZ1tEHovQJxWAAAAAElFTkSuQmCC";

        Texture2D tex = new Texture2D(2, 2);
        byte[] imageBytes = Convert.FromBase64String(icon);
        tex.LoadImage(imageBytes);

        GUIContent gUIContent = new GUIContent();
        gUIContent.text = "TileMap Editor";
        gUIContent.image = (Texture)tex;

        GetWindow(typeof(TileMapEditor)).titleContent = gUIContent;
        GetWindow(typeof(TileMapEditor)).Show();
    }

    public void OnEnable()
    {
        tileManager = FindObjectOfType<TileManager>();

        SceneView.duringSceneGui += OnSceneGUI;
    }

    public void OnDisable()
    {
        Destroy();

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            enablePlacement = true;
            enableDestroy = false;
            if (currentTile == null)
            {
                ResetTile();
            }
        }

        if (GUILayout.Button("Delete"))
        {
            enablePlacement = true;
            enableDestroy = true;
            if (currentTile == null)
            {
                ResetTile();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Search:");
        searchString = EditorGUILayout.TextField(searchString);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(0, true);

        scrollPositionSelection = EditorGUILayout.BeginScrollView(scrollPositionSelection);
        UpdateSelectionGrid(searchString);
        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            //ResetTileDefinition();
            SetSelectionDefinition();
        }
    }

    private void SetSelectionDefinition()
    {
        if (assetList.Count == 0)
            return;

        if (assetIndex >= assetList.Count)
            assetIndex = 0;

    }

    private void UpdateSelectionGrid(string searchString)
    {
        LoadAssets<Plenum>(searchString);

        GUIStyle style = new GUIStyle();
        style.imagePosition = ImagePosition.ImageAbove;
        style.contentOffset = new Vector2(0, 0);
        style.margin = new RectOffset(15, 15, 15, 15);
        style.padding = new RectOffset(10, 10, 10, 10);
        style.onNormal.background = Texture2D.grayTexture;

        assetIndex = GUILayout.SelectionGrid(assetIndex, assetIcons.ToArray(), 3, style);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (enablePlacement == false)
            return;
    }



    private void ResetTile()
    {
        GameObject obj = new GameObject("Ghost Tile");
        Tile tile = obj.AddComponent<Tile>();
        tile.SetSprite(floorSprite);
        tile.start();

        tile.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

        obj.transform.SetParent(tileManager.transform);
        obj.tag = "EditorOnly";

        currentTile = tile;

    }
    private void Destroy()
    {
        
    }

    public void LoadAssets<T>(string assetName = "") where T : TileBase
    {
        assetList.Clear();
        assetIcons.Clear();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}",typeof(T)));

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (assetName != "" && !asset.name.ToUpper().Contains(assetName.ToUpper()))
            {
                continue;
            }

            Texture2D texture = AssetPreview.GetAssetPreview(asset.prefab);
            assetIcons.Add(new GUIContent(asset.name, texture));
            assetList.Add(asset);
        }
    }
}
