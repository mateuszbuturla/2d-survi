using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WorldGenerationHelper
{
    public static int GetWorldSegmentSize(WorldGenerationData wgd)
    {
        return wgd.worldSize / wgd.segmentCount;
    }

    public static int GetWorldCenter(WorldGenerationData wgd)
    {
        return (int)(wgd.segmentCount / 2);
    }

    public static Dictionary<Vector3Int, TileBase> GetTilesFromTilemap(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        Dictionary<Vector3Int, TileBase> tiles = new Dictionary<Vector3Int, TileBase>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(pos);

                if (tile != null)
                {
                    tiles[pos] = tile;
                }
            }
        }

        return tiles;
    }

    public static BiomeData GetBiomeGenerator(List<BiomeData> biomeGeneratorsData, BiomeType biomeType)
    {
        return biomeGeneratorsData.Find(bg => bg.biomeType == biomeType);
    }
}
