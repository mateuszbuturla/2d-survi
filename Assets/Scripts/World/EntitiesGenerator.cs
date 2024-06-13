using System.Collections.Generic;
using UnityEngine;

public class EntitiesGenerator
{
    List<BiomeData> biomeGeneratorsData;
    Dictionary<Vector2Int, List<Biome>> biomeGrid;
    System.Random random;

    public EntitiesGenerator(List<BiomeData> biomeGeneratorsData, Dictionary<Vector2Int, List<Biome>> biomeGrid, ref System.Random random)
    {
        this.biomeGeneratorsData = biomeGeneratorsData;
        this.biomeGrid = biomeGrid;
        this.random = random;
    }

    public Vector2Int GetRandomPosInBiome(List<Biome> biomeParts)
    {
        int randomBiomePart = random.Next(0, biomeParts.Count);
        Biome biome = biomeParts[randomBiomePart];
        int randomTile = random.Next(0, biome.biomePoints.Count);
        Vector2Int tilePos = biome.biomePoints[randomTile];
        return tilePos;
    }

    public Dictionary<Vector2Int, GameObject> GenerateEntities()
    {
        Dictionary<Vector2Int, GameObject> entitiesPos = new Dictionary<Vector2Int, GameObject>();

        foreach (Vector2Int biomeMainPos in biomeGrid.Keys)
        {
            if (biomeGrid[biomeMainPos].Count == 0)
            {
                continue;
            }
            Biome firstBiome = biomeGrid[biomeMainPos][0];

            BiomeData biomeData = WorldGenerationHelper.GetBiomeGenerator(biomeGeneratorsData, firstBiome.biomeType);

            foreach (BiomeObjectData biomeObject in biomeData.biomeGenerator.objects)
            {
                if (!biomeObject.staticCount)
                {
                    List<Vector2Int> objectPositions = new List<Vector2Int>();

                    for (int i = 0; i < biomeObject.numSamplesBeforeRejection; i++)
                    {
                        Vector2Int randomPos = GetRandomPosInBiome(biomeGrid[biomeMainPos]);

                        if (IsValid(randomPos, objectPositions, biomeObject.minDistanceBetween))
                        {
                            objectPositions.Add(randomPos);
                            entitiesPos[randomPos] = biomeObject.prefab;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < biomeObject.count; i++)
                    {
                        Vector2Int randomPos = GetRandomPosInBiome(biomeGrid[biomeMainPos]);
                        entitiesPos[randomPos] = biomeObject.prefab;
                    }
                }
            }
        }

        return entitiesPos;
    }

    private bool IsValid(Vector2Int candidate, List<Vector2Int> points, int radius)
    {
        foreach (Vector2Int point in points)
        {
            float distance = Vector2Int.Distance(point, candidate);
            if (distance < radius)
            {
                return false;
            }
        }
        return true;
    }
}
