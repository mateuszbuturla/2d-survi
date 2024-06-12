using System.Collections.Generic;
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

    private Dictionary<Vector2Int, BiomeType> GenerateBiomesDistribution()
    {
        int minDistance = 4;

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

        List<BiomeType> biomeDistributionOrder = new List<BiomeType>();

        foreach (BiomeData biome in biomeGeneratorsData)
        {
            for (int i = 1; i < biome.count; i++)
            {
                biomeDistributionOrder.Add(biome.biomeType);
            }
        }

        foreach (BiomeType biomeType in biomeDistributionOrder)
        {
            Vector2Int randomPos = GetRandomPositionWithoutNearby(biomesDistribution, minDistance);
            biomesDistribution[randomPos] = biomeType;
        }

        return biomesDistribution;
    }

    private Vector2Int GetRandomPositionWithoutNearby(Dictionary<Vector2Int, BiomeType> biomesDistribution, int minDistance)
    {
        int attemptCount = 0;
        int maxAttempts = 1000;

        while (true)
        {
            int randomX = Random.Range(1, worldGenerationData.segmentCount - 1);
            int randomY = Random.Range(1, worldGenerationData.segmentCount - 1);

            Vector2Int randomPos = new Vector2Int(randomX, randomY);
            bool valid = true;

            foreach (var pos in biomesDistribution.Keys)
            {
                if (Vector2Int.Distance(randomPos, pos) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

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
