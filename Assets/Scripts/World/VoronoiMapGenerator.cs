using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VoronoiMapGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public VoronoiDistortionData voronoiDistortionData;

    public Tilemap tilemap;
    public Tilemap tilemapObjects;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

    public GameObject iceHouse1;

    [Range(-12312312, 12312312)]
    public int seed;

    void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        WorldGenerator wg = new WorldGenerator(biomeGeneratorsData, worldGenerationData, voronoiDistortionData, seed);

        WorldDataDto worldData = wg.GenerateWorld();

        FillTilemap(worldData.biomeTiles);

        foreach (Vector2Int entityPos in worldData.entities.Keys)
        {
            GenerateObject(worldData.entities[entityPos], entityPos);
        }
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

    (Tilemap, Tilemap) GetTilemapFromPrefab(GameObject prefab)
    {
        TileEntity te = prefab.GetComponent<TileEntity>();
        return (te.baseTileMap, te.additionalTileMap);
    }

    void GenerateObject(GameObject prefab, Vector2Int pos)
    {
        (Tilemap, Tilemap) tls = GetTilemapFromPrefab(prefab);
        Dictionary<Vector3Int, TileBase> tilesBase = WorldGenerationHelper.GetTilesFromTilemap(tls.Item1);
        Dictionary<Vector3Int, TileBase> tilesAdditional = WorldGenerationHelper.GetTilesFromTilemap(tls.Item2);

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