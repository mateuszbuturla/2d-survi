using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeDecorationRandom : TileDecorationHandler
{
    public List<TileBase> tile;
    [Range(0f, 100f)]
    public float threshold;
    public List<TileBase> allowedTilesToPlaceOn;

    protected override TileBase TryHandling(Vector2Int pos, System.Random random, TileBase worldTile)
    {
        float r = (float)(random.NextDouble() * 100);

        if (r >= threshold || !allowedTilesToPlaceOn.Contains(worldTile))
        {
            return null;
        }

        int tileIndex = MapFloatToTileIndex(r, threshold, tile.Count);

        return tile[tileIndex];
    }

    private int MapFloatToTileIndex(float value, float threshold, int tileCount)
    {
        float normalizedValue = value / threshold;

        int tileIndex = Mathf.FloorToInt(normalizedValue * tileCount);

        tileIndex = Mathf.Clamp(tileIndex, 0, tileCount - 1);

        return tileIndex;
    }
}
