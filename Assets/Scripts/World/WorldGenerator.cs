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

    System.Random random;
    public int seed;

    private float RandomRange(float min, float max)
    {
        return (float)(min + (max - min) * random.NextDouble());
    }

    public WorldGenerator(List<BiomeData> biomeGeneratorsData, WorldGenerationData worldGenerationData, VoronoiDistortionData voronoiDistortionData, int seed)
    {
        this.biomeGeneratorsData = biomeGeneratorsData;
        this.worldGenerationData = worldGenerationData;
        this.voronoiDistortionData = voronoiDistortionData;
        this.seed = seed;

        random = new System.Random(seed);
    }

    public Dictionary<Vector2Int, BiomeType> Test()
    {
        return biomeDistribution;
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

    // Function for generating a world sketch (selects where to place each biome)
    private Dictionary<Vector2Int, BiomeType> GenerateBiomesDistribution()
    {
        int minDistance = worldGenerationData.minDistanceBetweenBiomeCenters; // It means what is the minimum distance between the centers of biomes

        Dictionary<Vector2Int, BiomeType> biomesDistribution = new Dictionary<Vector2Int, BiomeType>();

        int half = worldGenerationData.segmentCount / 2;

        for (int x = -half; x < half; x++)
        {
            for (int y = -half; y < half; y++)
            {
                if (x == -half || y == -half || x == half - 1 || y == half - 1) // Generate water on the world edges
                {
                    biomesDistribution[new Vector2Int(x, y)] = BiomeType.WATER;
                }
            }
        }

        int centerIndex = 0;
        biomesDistribution[new Vector2Int(centerIndex, centerIndex)] = BiomeType.GRASSLAND; // Sets default biome at the center of a world


        // Generate inner biomes
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
        // Generate inner biomes

        // Generate outer biomes
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
        // Generate outer biomes

        return biomesDistribution;
    }

    private Vector2Int ConvertToGridPoint(Vector2Int center, Vector2 randomPoint)
    {
        int half = worldGenerationData.segmentCount / 2;

        int x = Mathf.Clamp(center.x + Mathf.RoundToInt(randomPoint.x), -half, half - 1);
        int y = Mathf.Clamp(center.y + Mathf.RoundToInt(randomPoint.y), -half, half - 1);

        return new Vector2Int(x, y);
    }

    // Function for finding where a new biome can be placed
    private Vector2Int GetRandomPositionWithoutNearby(Dictionary<Vector2Int, BiomeType> biomesDistribution, int minDistance, BiomeTier biomeTier)
    {
        int attemptCount = 0;
        int maxAttempts = 3000;

        Vector2Int center = new Vector2Int(0, 0);

        while (true)
        {
            bool valid = true;

            float angle = (float)(random.NextDouble() * (Mathf.PI * 2));
            float distance;

            if (biomeTier == BiomeTier.INNER)
            {
                distance = (float)RandomRange(0, worldGenerationData.outerRingDistance); // Generate random position for inner biome
            }
            else
            {
                distance = (float)RandomRange(worldGenerationData.outerRingDistance, worldGenerationData.segmentCount / 2 - worldGenerationData.borderThickness); // Generate random position for outer biome
            }

            float x = distance * Mathf.Cos(angle);
            float y = distance * Mathf.Sin(angle);

            Vector2Int randomPos = ConvertToGridPoint(center, new Vector2(x, y));

            if (valid)
            {
                foreach (var pos in biomesDistribution.Keys)
                {
                    if (Vector2Int.Distance(randomPos, pos) < minDistance)
                    {
                        valid = false;
                        break;
                    }
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

    // Function for generating actual biomes: after receiving data about each biome's location, this function uses a Voronoi diagram to determine the closest biome, aiming to create a more chaotic world
    private List<Biome> GenerateBiomeBase()
    {
        List<Biome> generatedBiomes = new List<Biome>();

        int segmentSize = WorldGenerationHelper.GetWorldSegmentSize(worldGenerationData);
        int half = worldGenerationData.segmentCount / 2;

        for (int xSegment = -half; xSegment < half; xSegment++)
        {
            for (int ySegment = -half; ySegment < half; ySegment++)
            {
                int startX = xSegment * segmentSize;
                int endX = (xSegment + 1) * segmentSize;
                int startY = ySegment * segmentSize;
                int endY = (ySegment + 1) * segmentSize;

                BiomeType biomeType = GetClosestBiomeTypeFromDistribution(new Vector2Int(xSegment, ySegment)); // Gets biome type from biomeDistribution

                int x = random.Next(startX, endX);
                int y = random.Next(startY, endY);

                Vector2Int baseBiomePoint = new Vector2Int(x, y);

                generatedBiomes.Add(new Biome(biomeType, baseBiomePoint));
            }
        }

        return generatedBiomes;
    }

    // Function for finding closest biome from biomeDistribution (it uses voronoi diagram)
    private BiomeType GetClosestBiomeTypeFromDistribution(Vector2Int position)
    {
        BiomeType closestPosition = BiomeType.WATER;
        float closestDistance = Mathf.Infinity;

        foreach (Vector2Int biomePosition in biomeDistribution.Keys)
        {
            BiomeData biomData = biomeGeneratorsData.Find(biome => biome.biomeType == biomeDistribution[biomePosition]);

            float distance = Vector2Int.Distance(biomePosition, position); // The distance between the location and the currently checked biome
            float distanceToCenter = Vector2Int.Distance(position, new Vector2Int(0, 0)); // Distance to center of the world 

            if (distance < closestDistance && distance < worldGenerationData.maxDistanceFromBiomeCenter)
            {
                if (biomData.biomeTier == BiomeTier.INNER && distanceToCenter > worldGenerationData.outerRingDistance) { }
                else if (biomData.biomeTier == BiomeTier.OUTER && distanceToCenter < worldGenerationData.outerRingDistance) { }
                else
                {
                    closestDistance = distance;
                    closestPosition = biomeDistribution[biomePosition];
                }
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

    // Function for generating each tile based on the closest biome using voronoi diagram
    private Dictionary<Vector2Int, TileBase> GenerateMap()
    {
        Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

        int half = worldGenerationData.worldSize / 2;

        for (int x = -half; x < half; x++)
        {
            for (int y = -half; y < half; y++)
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

    // Function for finding the closest biome (it uses voronoi diagram)
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
