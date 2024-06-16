using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Vector2Int pos;
    public Dictionary<Vector2Int, Biomes> tiles;

    public Chunk(Vector2Int pos)
    {
        this.pos = pos;
        this.tiles = new Dictionary<Vector2Int, Biomes>();
    }
}