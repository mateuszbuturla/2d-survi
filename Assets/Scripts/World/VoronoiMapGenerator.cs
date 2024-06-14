using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VoronoiMapGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public VoronoiDistortionData voronoiDistortionData;

    public Tilemap tilemap;
    public Tilemap tilemapObjects;
    public Tilemap tilemapDecoration;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

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
        FillDecorations(worldData.decorationsTiles);

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

    void FillDecorations(Dictionary<Vector2Int, TileBase> tiles)
    {
        foreach (KeyValuePair<Vector2Int, TileBase> tileEntry in tiles)
        {
            Vector2Int position = tileEntry.Key;
            TileBase tile = tileEntry.Value;

            tilemapDecoration.SetTile(new Vector3Int(position.x, position.y, 0), tile);
        }

        tilemapDecoration.RefreshAllTiles();
    }

    void GenerateObject(GameObject prefab, Vector2Int pos)
    {
        Vector3Int basePosition = new Vector3Int(pos.x, pos.y, 0);

        Vector3 worldPosition = tilemap.CellToWorld(basePosition);

        Instantiate(prefab, worldPosition, Quaternion.identity);
    }
}