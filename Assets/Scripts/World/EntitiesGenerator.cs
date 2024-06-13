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
                for (int i = 0; i < biomeObject.count; i++)
                {
                    int randomBiomePart = random.Next(0, biomeGrid[biomeMainPos].Count);
                    Biome biome = biomeGrid[biomeMainPos][randomBiomePart];
                    int randomTile = random.Next(0, biome.biomePoints.Count);
                    Vector2Int tile = biome.biomePoints[randomTile];

                    entitiesPos[tile] = biomeObject.prefab;
                }
            }
        }

        return entitiesPos;
    }
}
