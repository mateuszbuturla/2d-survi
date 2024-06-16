using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class BiomeGenerationData
{
    [Range(0f, 1f)]
    public float temperature;
    [Range(0f, 1f)]
    public float humidity;
    [Range(0f, 1f)]
    public float temperatureMax;
    [Range(0f, 1f)]
    public float humidityMax;
    public Biomes biome;
}
