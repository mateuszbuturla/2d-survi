using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator
{
    WorldGenerationData worldGenerationData;
    List<BiomeData> biomeGeneratorsData = new List<BiomeData>();
    VoronoiDistortionData voronoiDistortionData;

    Dictionary<Vector2Int, BiomeType> biomeDistribution;
    Dictionary<Vector2Int, List<Biome>> biomeGrid;
    List<Biome> biomes;

    public WorldGenerator(List<BiomeData> biomeGeneratorsData, WorldGenerationData worldGenerationData, VoronoiDistortionData voronoiDistortionData)
    {
        this.biomeGeneratorsData = biomeGeneratorsData;
        this.worldGenerationData = worldGenerationData;
        this.voronoiDistortionData = voronoiDistortionData;
    }

    public Dictionary<Vector2Int, TileBase> GenerateWorld()
    {
        Dictionary<Vector2Int, TileBase> tiles;

        biomeDistribution = GenerateBiomesDistribution();

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        biomes = GenerateBiomeBase();
        stopwatch.Stop();
        Debug.Log($"GenerateBiomeBase: {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Start();
        biomeGrid = CreateBiomeGrid();
        stopwatch.Stop();
        Debug.Log($"CreateBiomeGrid: {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Start();
        tiles = GenerateMap();
        stopwatch.Stop();
        Debug.Log($"GenerateMap: {stopwatch.ElapsedMilliseconds} ms");

        return tiles;
    }

    private List<BiomeData> FilterBiomeDataByTier(BiomeTier biomeTier)
    {
        return (from biome in biomeGeneratorsData
                where biome.biomeTier == biomeTier
                select biome).ToList();
    }

    private Dictionary<Vector2Int, BiomeType> GenerateBiomesDistribution()
    {
        int minDistance = 0;

        Dictionary<Vector2Int, BiomeType> biomesDistribution = new Dictionary<Vector2Int, BiomeType>();

        for (int x = 0; x < worldGenerationData.segmentCount; x++)
        {
            for (int y = 0; y < worldGenerationData.segmentCount; y++)
            {
                if (x == 0 || y == 0 || x == worldGenerationData.segmentCount - 1 || y == worldGenerationData.segmentCount - 1)
                {
                    biomesDistribution[new Vector2Int(x, y)] = BiomeType.WATER;
                }
            }
        }

        int centerIndex = worldGenerationData.segmentCount / 2;
        biomesDistribution[new Vector2Int(centerIndex, centerIndex)] = BiomeType.GRASSLAND;

        List<BiomeType> biomeDistributionOrderInner = new List<BiomeType>();

        foreach (BiomeData biome in FilterBiomeDataByTier(BiomeTier.INNER))
        {
            for (int i = 0; i < biome.count; i++)
            {
                biomeDistributionOrderInner.Add(biome.biomeType);
            }
        }

        foreach (BiomeType biomeType in biomeDistributionOrderInner)
        {
            Vector2Int randomPos = GetRandomPositionWithoutNearby(biomesDistribution, minDistance, BiomeTier.INNER);
            biomesDistribution[randomPos] = biomeType;
        }

        List<BiomeType> biomeDistributionOrderOuter = new List<BiomeType>();

        foreach (BiomeData biome in FilterBiomeDataByTier(BiomeTier.OUTER))
        {
            for (int i = 0; i < biome.count; i++)
            {
                biomeDistributionOrderOuter.Add(biome.biomeType);
            }
        }

        foreach (BiomeType biomeType in biomeDistributionOrderOuter)
        {
            Vector2Int randomPos = GetRandomPositionWithoutNearby(biomesDistribution, minDistance, BiomeTier.OUTER);
            biomesDistribution[randomPos] = biomeType;
        }

        return biomesDistribution;
    }

    private Vector2Int ConvertToGridPoint(Vector2Int center, Vector2 randomPoint)
    {
        int x = Mathf.Clamp(center.x + Mathf.RoundToInt(randomPoint.x), 0, worldGenerationData.segmentCount - 1);
        int y = Mathf.Clamp(center.y + Mathf.RoundToInt(randomPoint.y), 0, worldGenerationData.segmentCount - 1);

        return new Vector2Int(x, y);
    }

    private Vector2Int GetRandomPositionWithoutNearby(Dictionary<Vector2Int, BiomeType> biomesDistribution, int minDistance, BiomeTier biomeTier)
    {
        int attemptCount = 0;
        int maxAttempts = 3000;

        Vector2Int center = new Vector2Int(worldGenerationData.segmentCount / 2, worldGenerationData.segmentCount / 2);

        while (true)
        {
            int randomX = Random.Range(1, worldGenerationData.segmentCount - 1);
            int randomY = Random.Range(1, worldGenerationData.segmentCount - 1);

            //Vector2Int randomPos = new Vector2Int(randomX, randomY);

            bool valid = true;

            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance;

            if (biomeTier == BiomeTier.INNER)
            {
                distance = Random.Range(1, 9);
            }
            else
            {
                distance = Random.Range(9, 10);
            }

            float x = distance * Mathf.Cos(angle);
            float y = distance * Mathf.Sin(angle);

            Vector2Int randomPos = ConvertToGridPoint(center, new Vector2(x, y));

            Debug.Log(randomPos);

            // if (biomeTier == BiomeTier.INNER && distance >= worldGenerationData.outerRingDistance)
            // {
            //     valid = false;
            // }
            // if (biomeTier == BiomeTier.OUTER && distance < worldGenerationData.outerRingDistance)
            // {
            //     valid = false;
            // }

            // if (valid)
            // {
            //     foreach (var pos in biomesDistribution.Keys)
            //     {
            //         if (Vector2Int.Distance(randomPos, pos) < minDistance)
            //         {
            //             valid = false;
            //             break;
            //         }
            //     }
            // }

            if (valid)
            {
                return randomPos;
            }

            attemptCount++;
            if (attemptCount >= maxAttempts)
            {
                Debug.LogError("Cannot find matching position");
                return Vector2Int.zero;
            }
        }
    }

    private List<Biome> GenerateBiomeBase()
    {
        List<Biome> generatedBiomes = new List<Biome>();

        int segmentSize = WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData);

        for (int xSegment = 0; xSegment < worldGenerationData.segmentCount; xSegment++)
        {
            for (int ySegment = 0; ySegment < worldGenerationData.segmentCount; ySegment++)
            {
                int startX = xSegment * segmentSize;
                int endX = (xSegment + 1) * segmentSize;
                int startY = ySegment * segmentSize;
                int endY = (ySegment + 1) * segmentSize;

                BiomeType biomeType = GetClosestBiomeTypeFromDistribution(new Vector2Int(xSegment, ySegment));

                int x = Random.Range(startX, endX);
                int y = Random.Range(startY, endY);

                Vector2Int baseBiomePoint = new Vector2Int(x, y);

                generatedBiomes.Add(new Biome(biomeType, baseBiomePoint));
            }
        }

        return generatedBiomes;
    }

    private BiomeType GetClosestBiomeTypeFromDistribution(Vector2Int position)
    {
        BiomeType closestPosition = BiomeType.WATER;
        float closestDistance = Mathf.Infinity;

        foreach (Vector2Int biomePosition in biomeDistribution.Keys)
        {
            float distance = Vector2Int.Distance(biomePosition, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = biomeDistribution[biomePosition];
            }
        }

        return closestPosition;
    }

    private Dictionary<Vector2Int, List<Biome>> CreateBiomeGrid()
    {
        Dictionary<Vector2Int, List<Biome>> biomeGrid = new Dictionary<Vector2Int, List<Biome>>();

        int segmentSize = WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData);

        foreach (Biome biome in biomes)
        {
            Vector2Int gridPos = new Vector2Int(biome.mainBiomePoint.x / segmentSize, biome.mainBiomePoint.y / segmentSize);

            if (!biomeGrid.ContainsKey(gridPos))
            {
                biomeGrid[gridPos] = new List<Biome>();
            }

            biomeGrid[gridPos].Add(biome);
        }

        return biomeGrid;
    }

    private Dictionary<Vector2Int, TileBase> GenerateMap()
    {
        Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

        for (int x = 0; x < worldGenerationData.worldSize; x++)
        {
            for (int y = 0; y < worldGenerationData.worldSize; y++)
            {
                Biome closestBiome = FindClosestBiome(new Vector3Int(x, y, 0));
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

    private Biome FindClosestBiome(Vector3Int position)
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
