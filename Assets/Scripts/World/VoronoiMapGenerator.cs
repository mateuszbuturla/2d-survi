using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class VoronoiMapGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public VoronoiDistortionData voronoiDistortionData;

    public Tilemap tilemap;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

    public GenerateBiome roadGenerator;

    public List<Biome> biomes;

    public GameObject tree;

    private Dictionary<Vector2Int, List<Biome>> biomeGrid;

    void Start()
    {
        WorldGenerator wg = new WorldGenerator(biomeGeneratorsData, worldGenerationData, voronoiDistortionData);

        Dictionary<Vector2Int, TileBase> tiles = wg.GenerateWorld();

        FillTilemap(tiles);
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
}