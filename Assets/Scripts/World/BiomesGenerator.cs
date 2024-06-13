using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BiomesGenerator
{
    List<BiomeData> biomeGeneratorsData;
    WorldGenerationData worldGenerationData;
    System.Random random;
    private Vector2Int worldCenter = new Vector2Int(0, 0);

    public BiomesGenerator(List<BiomeData> biomeGeneratorsData, WorldGenerationData worldGenerationData, ref System.Random random)
    {
        this.biomeGeneratorsData = biomeGeneratorsData;
        this.worldGenerationData = worldGenerationData;
        this.random = random;
    }

    public Dictionary<Vector2Int, List<Biome>> GenerateBiomes()
    {
        Dictionary<Vector2Int, BiomeType> biomeDistribution = GenerateBiomesDistribution();
        Dictionary<Vector2Int, List<Biome>> biomes = GenerateBiomeBase(biomeDistribution);

        return biomes;
    }

    private float RandomRange(float min, float max)
    {
        return (float)(min + (max - min) * random.NextDouble());
    }

    #region Generating biomes sketch

    // Function for generating a world sketch (selects where to place each biome)
    private Dictionary<Vector2Int, BiomeType> GenerateBiomesDistribution()
    {
        int minDistance = worldGenerationData.minDistanceBetweenBiomeCenters; // It means what is the minimum distance between the centers of the biomes

        Dictionary<Vector2Int, BiomeType> biomesDistribution = new Dictionary<Vector2Int, BiomeType>();
        biomesDistribution[worldCenter] = BiomeType.GRASSLAND; // Sets default biome at the center of a world


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

    // Function for finding where a new biome can be placed
    private Vector2Int GetRandomPositionWithoutNearby(Dictionary<Vector2Int, BiomeType> biomesDistribution, int minDistance, BiomeTier biomeTier)
    {
        int attemptCount = 0;
        int maxAttempts = 3000;

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

            Vector2Int randomPos = ConvertToGridPoint(worldCenter, new Vector2(x, y));

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

    private Vector2Int ConvertToGridPoint(Vector2Int center, Vector2 randomPoint)
    {
        int half = worldGenerationData.segmentCount / 2;

        int x = Mathf.Clamp(center.x + Mathf.RoundToInt(randomPoint.x), -half, half - 1);
        int y = Mathf.Clamp(center.y + Mathf.RoundToInt(randomPoint.y), -half, half - 1);

        return new Vector2Int(x, y);
    }

    private List<BiomeData> FilterBiomeDataByTier(BiomeTier biomeTier)
    {
        return (from biome in biomeGeneratorsData
                where biome.biomeTier == biomeTier
                select biome).ToList();
    }

    #endregion

    #region Generating actial biomes

    // Function for generating actual biomes: after receiving data about each biome's location, this function uses a Voronoi diagram to determine the closest biome, aiming to create a more chaotic world
    private Dictionary<Vector2Int, List<Biome>> GenerateBiomeBase(Dictionary<Vector2Int, BiomeType> biomeDistribution)
    {
        Dictionary<Vector2Int, List<Biome>> generatedBiomes = new Dictionary<Vector2Int, List<Biome>>();

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

                (Vector2Int, BiomeType) biomeData = GetClosestBiomeTypeFromDistribution(biomeDistribution, new Vector2Int(xSegment, ySegment)); // Gets biome type from biomeDistribution

                int x = random.Next(startX, endX);
                int y = random.Next(startY, endY);

                Vector2Int baseBiomePoint = new Vector2Int(x, y);
                Vector2Int segmentPost = new Vector2Int(xSegment, ySegment);

                if (!generatedBiomes.ContainsKey(biomeData.Item1))
                {
                    generatedBiomes[biomeData.Item1] = new List<Biome>();
                }

                generatedBiomes[biomeData.Item1].Add(new Biome(biomeData.Item2, baseBiomePoint, segmentPost));
            }
        }

        return generatedBiomes;
    }

    private (Vector2Int, BiomeType) GetClosestBiomeTypeFromDistribution(Dictionary<Vector2Int, BiomeType> biomeDistribution, Vector2Int position)
    {
        Vector2Int closestPosition = Vector2Int.zero;
        BiomeType closetType = BiomeType.WATER;
        float closestDistance = Mathf.Infinity;

        foreach (Vector2Int biomePosition in biomeDistribution.Keys)
        {
            BiomeData biomData = biomeGeneratorsData.Find(biome => biome.biomeType == biomeDistribution[biomePosition]);

            float distance = Vector2Int.Distance(biomePosition, position); // The distance between the location and the currently checked biome
            float distanceToCenter = Vector2Int.Distance(position, worldCenter); // Distance to center of the world 

            if (distance < closestDistance && distance < worldGenerationData.maxDistanceFromBiomeCenter)
            {
                if (biomData.biomeTier == BiomeTier.INNER && distanceToCenter > worldGenerationData.outerRingDistance) { }
                else if (biomData.biomeTier == BiomeTier.OUTER && distanceToCenter < worldGenerationData.outerRingDistance) { }
                else
                {
                    closestDistance = distance;
                    closetType = biomeDistribution[biomePosition];
                    closestPosition = biomePosition;
                }
            }
        }

        return (closestPosition, closetType);
    }

    #endregion
}
