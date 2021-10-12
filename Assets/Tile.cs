using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /* tmp code start */

    public Sprite floorSprite;
    public SpriteRenderer renderer;

    public void start()
    {
        renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = floorSprite;
        renderer.drawMode = SpriteDrawMode.Sliced;
        renderer.size = new Vector2(1, 1);
    }

    public void SetSprite(Sprite sprite)
    {
        this.floorSprite = sprite;
    } 

    /* end tmp code */
}
