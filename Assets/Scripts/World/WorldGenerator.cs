using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator
{
    WorldGenerationData worldGenerationData;
    List<BiomeData> biomeGeneratorsData = new List<BiomeData>();
    VoronoiDistortionData voronoiDistortionData;
    System.Random random;
    public int seed;

    public WorldGenerator(List<BiomeData> biomeGeneratorsData, WorldGenerationData worldGenerationData, VoronoiDistortionData voronoiDistortionData, int seed)
    {
        this.biomeGeneratorsData = biomeGeneratorsData;
        this.worldGenerationData = worldGenerationData;
        this.voronoiDistortionData = voronoiDistortionData;
        this.seed = seed;

        random = new System.Random(seed);
    }

    public Dictionary<Vector2Int, TileBase> GenerateWorld()
    {
        BiomesGenerator biomesGenerator = new BiomesGenerator(biomeGeneratorsData, worldGenerationData, ref random);
        Dictionary<Vector2Int, List<Biome>> biomeGrid = biomesGenerator.GenerateBiomes();
        Dictionary<Vector2Int, TileBase> tiles = GenerateMap(biomeGrid);
        return tiles;
    }

    // Function for generating each tile based on the closest biome using voronoi diagram
    private Dictionary<Vector2Int, TileBase> GenerateMap(Dictionary<Vector2Int, List<Biome>> biomeGrid)
    {
        Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

        int half = worldGenerationData.worldSize / 2;

        for (int x = -half; x < half; x++)
        {
            for (int y = -half; y < half; y++)
            {
                Biome closestBiome = FindClosestBiome(biomeGrid, new Vector3Int(x, y, 0));
                BiomeData biomeGenerator = biomeGeneratorsData.Find(bg => bg.biomeType == closestBiome.biomeType);
                Vector2Int pos = new Vector2Int(x, y);

                if (closestBiome != null)
                {
                    closestBiome.biomePoints.Add(pos);
                }

                tiles[pos] = biomeGenerator.biomeGenerator.baseTile;
            }
        }

        return tiles;
    }

    // Function for finding the closest biome (it uses voronoi diagram)
    private Biome FindClosestBiome(Dictionary<Vector2Int, List<Biome>> biomeGrid, Vector3Int position)
    {
        Biome closestBiome = null;
        float closestDistance = float.MaxValue;
        int segmentSize = WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData);

        Vector3Int newPos = position + VoronoiEdgeDistortion.Get2DTurbulence(new Vector2Int(position.x, position.y), voronoiDistortionData);

        Vector2Int gridPos = new Vector2Int(position.x / segmentSize, position.y / segmentSize);

        List<Biome> nearbyBiomes = new List<Biome>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighborPos = gridPos + new Vector2Int(x, y);
                if (biomeGrid.ContainsKey(neighborPos))
                {
                    nearbyBiomes.AddRange(biomeGrid[neighborPos]);
                }
            }
        }

        foreach (Biome biome in nearbyBiomes)
        {
            float distance = Vector3Int.Distance(newPos, new Vector3Int(biome.mainBiomePoint.x, biome.mainBiomePoint.y, 0));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBiome = biome;
            }
        }

        return closestBiome;
    }
}
