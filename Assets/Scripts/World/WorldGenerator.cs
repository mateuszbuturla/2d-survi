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

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        Dictionary<Vector2Int, List<Biome>> biomeGrid = biomesGenerator.GenerateBiomes();
        stopwatch.Stop();
        Debug.Log($"GenerateBiomes: {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Reset();
        stopwatch.Start();
        Dictionary<Vector2Int, TileBase> tiles = GenerateMap(biomeGrid);
        stopwatch.Stop();
        Debug.Log($"GenerateMap: {stopwatch.ElapsedMilliseconds} ms");


        stopwatch.Reset();
        stopwatch.Start();
        foreach (var b in biomeGrid.Keys)
        {
            if (biomeGrid[b].Count > 0 && biomeGrid[b][0].biomeType == BiomeType.TUNDRA)
            {
                Biome d = biomeGrid[b][0];

                for (int i = 0; i < 1; i++)
                {
                    int randomIndex = random.Next(0, d.biomePoints.Count);

                    tiles[d.biomePoints[randomIndex]] = null;
                }
            }
        }
        stopwatch.Stop();
        Debug.Log($"Random objects: {stopwatch.ElapsedMilliseconds} ms");

        return tiles;
    }

    // Function for generating each tile based on the closest biome using voronoi diagram
    private Dictionary<Vector2Int, TileBase> GenerateMap(Dictionary<Vector2Int, List<Biome>> biomeGrid)
    {
        Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

        Dictionary<Vector2Int, Biome> simplifiedBiomes = new Dictionary<Vector2Int, Biome>();
        foreach (Vector2Int e in biomeGrid.Keys)
        {
            foreach (Biome biome in biomeGrid[e])
            {
                simplifiedBiomes[biome.segmentPos] = biome;
            }
        }


        int half = worldGenerationData.worldSize / 2;

        for (int x = -half; x < half; x++)
        {
            for (int y = -half; y < half; y++)
            {
                Biome closestBiome = FindClosestBiome(simplifiedBiomes, new Vector3Int(x, y, 0));
                if (closestBiome != null)
                {
                    BiomeData biomeGenerator = biomeGeneratorsData.Find(bg => bg.biomeType == closestBiome.biomeType);
                    Vector2Int pos = new Vector2Int(x, y);

                    if (closestBiome != null)
                    {
                        closestBiome.biomePoints.Add(pos);
                    }

                    tiles[pos] = biomeGenerator.biomeGenerator.GetTile(pos);
                }
            }
        }

        return tiles;
    }

    // Function for finding the closest biome (it uses voronoi diagram)
    private Biome FindClosestBiome(Dictionary<Vector2Int, Biome> simplifiedBiomes, Vector3Int position)
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
                if (simplifiedBiomes.ContainsKey(neighborPos))
                {
                    nearbyBiomes.Add(simplifiedBiomes[neighborPos]);
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
