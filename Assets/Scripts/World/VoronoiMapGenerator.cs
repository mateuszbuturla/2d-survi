using System.Collections.Generic;
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

    [Range(-12312312, 12312312)]
    public int seed;

    void Start()
    {
        GenerateWorld();

        GenerateObject(iceHouse1, new Vector2Int(100, 100));
    }

    public void GenerateWorld()
    {
        // ClearDebugTilemap();
        WorldGenerator wg = new WorldGenerator(biomeGeneratorsData, worldGenerationData, voronoiDistortionData, seed);

        Dictionary<Vector2Int, TileBase> tiles = wg.GenerateWorld();

        FillTilemap(tiles);

        // Dictionary<Vector2Int, BiomeType> test = wg.Test();

        // foreach (Vector2Int pos in test.Keys)
        // {
        //     Vector2Int scaled = new Vector2Int(pos.x * WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData), pos.y * WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData));
        //     for (int x = scaled.x - 10; x < scaled.x + 10; x++)
        //     {
        //         for (int y = scaled.y - 10; y < scaled.y + 10; y++)
        //         {
        //             BiomeData biomeData = biomeGeneratorsData.Find(i => i.biomeType == test[pos]);
        //             tilemapObjects.SetTile(new Vector3Int(x, y, 0), biomeData.biomeGenerator.baseTile);
        //         }
        //     }
        // }
    }

    // void ClearDebugTilemap()
    // {
    //     BoundsInt bounds = tilemapObjects.cellBounds;

    //     for (int x = bounds.xMin; x < bounds.xMax; x++)
    //     {
    //         for (int y = bounds.yMin; y < bounds.yMax; y++)
    //         {
    //             for (int z = bounds.zMin; z < bounds.zMax; z++)
    //             {
    //                 Vector3Int position = new Vector3Int(x, y, z);
    //                 tilemapObjects.SetTile(position, null);
    //             }
    //         }
    //     }

    //     tilemapObjects.RefreshAllTiles();
    // }

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

    (Tilemap, Tilemap) GetTilemapFromPrefab(GameObject prefab)
    {
        TileEntity te = prefab.GetComponent<TileEntity>();
        return (te.baseTileMap, te.additionalTileMap);
    }

    void GenerateObject(GameObject prefab, Vector2Int pos)
    {
        (Tilemap, Tilemap) tls = GetTilemapFromPrefab(prefab);
        Dictionary<Vector3Int, TileBase> tilesBase = GetTilesFromPrefab(tls.Item1);
        Dictionary<Vector3Int, TileBase> tilesAdditional = GetTilesFromPrefab(tls.Item2);

        Vector3Int basePosition = new Vector3Int(pos.x, pos.y, 0);

        foreach (Vector3Int tilePos in tilesBase.Keys)
        {
            tilemap.SetTile(basePosition + tilePos, tilesBase[tilePos]);
        }

        foreach (Vector3Int tilePos in tilesAdditional.Keys)
        {
            tilemapObjects.SetTile(basePosition + tilePos, tilesAdditional[tilePos]);
        }

    }
}