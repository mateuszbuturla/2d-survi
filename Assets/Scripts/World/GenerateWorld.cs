using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct BiomeData
{
    [Range(0f, 1f)]
    public float startThreshold,
        endThreshold;
    public GenerateBiome biomeGenerator;
}

public class GenerateWorld : MonoBehaviour
{
    public NoiseSettings noiseSettings;
    public WorldGenerationData worldGenerationData;
    public Tilemap tilemap;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

    private int width;
    private int height;

    public GenerateBiome generateBiome;

    void Start()
    {
        this.width = this.worldGenerationData.worldWidth;
        this.height = this.worldGenerationData.worldHeight;
        GenerateTilemap();
    }

    void GenerateTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width;
                float yCoord = (float)y / height;


                float noiseValue = MyNoise.OctavePerlin(
                   x,
                   y,
                   noiseSettings
                );

                BiomeData biome = SelectBiomeData(noiseValue);
                biome.biomeGenerator.GenerateTile(new Vector3Int(x, y, 0));
            }
        }
    }

    BiomeData SelectBiomeData(float noiseValue)
    {
        foreach (var data in biomeGeneratorsData)
        {
            if (
                noiseValue >= data.startThreshold &&
                noiseValue < data.endThreshold
            )
                return data;
        }
        return biomeGeneratorsData[0];
    }
}
