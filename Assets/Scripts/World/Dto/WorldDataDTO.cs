using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldDataDto
{
    public Dictionary<Vector2Int, TileBase> biomeTiles;
    public Dictionary<Vector2Int, TileBase> decorationsTiles;
    public Dictionary<Vector2Int, GameObject> entities;

    public WorldDataDto(Dictionary<Vector2Int, TileBase> biomeTiles, Dictionary<Vector2Int, GameObject> entities, Dictionary<Vector2Int, TileBase> decorationsTiles)
    {
        this.biomeTiles = biomeTiles;
        this.entities = entities;
        this.decorationsTiles = decorationsTiles;
    }
}
