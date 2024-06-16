using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public Vector2Int pos;
    public Dictionary<Vector2Int, TileBase> tiles;
    public Dictionary<Vector2Int, TileBase> decorationTiles;

    public Chunk(Vector2Int pos)
    {
        this.pos = pos;
        this.tiles = new Dictionary<Vector2Int, TileBase>();
        this.decorationTiles = new Dictionary<Vector2Int, TileBase>();
    }
}
