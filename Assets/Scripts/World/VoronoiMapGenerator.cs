﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class VoronoiMapGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public VoronoiDistortionData voronoiDistortionData;

    public Tilemap tilemap;
    public Tilemap tilemapObjects;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

    public GenerateBiome roadGenerator;

    public List<Biome> biomes;

    public GameObject tree;

    private Dictionary<Vector2Int, List<Biome>> biomeGrid;

    public GameObject iceHouse1;

    void Start()
    {
        WorldGenerator wg = new WorldGenerator(biomeGeneratorsData, worldGenerationData, voronoiDistortionData);

        Dictionary<Vector2Int, TileBase> tiles = wg.GenerateWorld();

        FillTilemap(tiles);

        Dictionary<Vector2Int, BiomeType> test = wg.Test();

        foreach (var r in test.Keys)
        {
            var dd = test[r];
            UnityEngine.Debug.Log($"{test}:{dd}");
        }

        foreach (Vector2Int pos in test.Keys)
        {
            Vector2Int scaled = new Vector2Int(pos.x * WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData), pos.y * WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData));
            for (int x = scaled.x - 10; x < scaled.x + 10; x++)
            {
                for (int y = scaled.y - 10; y < scaled.y + 10; y++)
                {
                    BiomeData biomeData = biomeGeneratorsData.Find(i => i.biomeType == test[pos]);
                    tilemapObjects.SetTile(new Vector3Int(x, y, 0), biomeData.biomeGenerator.baseTile);
                }
            }
        }

        // GenerateIceHouse1();
    }

    void FillTilemap(Dictionary<Vector2Int, TileBase> tiles)
    {
        foreach (KeyValuePair<Vector2Int, TileBase> tileEntry in tiles)
        {
            Vector2Int position = tileEntry.Key;
            TileBase tile = tileEntry.Value;

            tilemap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
        }

        tilemap.RefreshAllTiles();
    }

    Dictionary<Vector3Int, TileBase> GetTilesFromPrefab(Tilemap tilemap)
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

    Tilemap GetTilemapFromPrefab(GameObject prefab)
    {
        return prefab.transform.GetChild(0).GetComponent<Tilemap>();
    }

    void GenerateIceHouse1()
    {
        Tilemap tl = GetTilemapFromPrefab(iceHouse1);
        Dictionary<Vector3Int, TileBase> tiles = GetTilesFromPrefab(tl);

        Vector3Int basePosition = new Vector3Int(250, 250, 0);

        foreach (Vector3Int pos in tiles.Keys)
        {
            tilemapObjects.SetTile(basePosition + pos, tiles[pos]);
        }

    }
}