using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public NoiseSettings noiseSettings;
    public NoiseSettings temperatureNoiseSettings;
    public NoiseSettings humidityNoiseSettings;
    public VoronoiDistortionData distortionNoiseSettings;

    public Tilemap tilemap;

    public TileBase waterTile;
    public TileBase forestTile;
    public TileBase grasslandTile;
    public TileBase snowTile;
    public TileBase desertTile;
    public TileBase jungleTile;

    [SerializeField]
    public List<BiomeGenerationData> biomes;

    private void ClearTilemap()
    {
        tilemap.ClearAllTiles();
        tilemap.RefreshAllTiles();
    }

    public void CreateWorld()
    {
        ClearTilemap();
        Dictionary<Vector2Int, Biomes> worldData = GenerateWorld();

        foreach (var kvp in worldData)
        {
            Vector3Int pos = new Vector3Int(kvp.Key.x, kvp.Key.y, 0);
            tilemap.SetTile(pos, GetTile(kvp.Value));
        }
    }

    private TileBase GetTile(Biomes biome)
    {
        if (biome == Biomes.GRASSLAND)
        {
            return grasslandTile;
        }
        if (biome == Biomes.DESERT)
        {
            return desertTile;
        }
        if (biome == Biomes.SNOW)
        {
            return snowTile;
        }
        if (biome == Biomes.FOREST)
        {
            return forestTile;
        }
        if (biome == Biomes.JUNGLE)
        {
            return jungleTile;
        }

        return waterTile;
    }

    public BiomeGenerationData SelectBiome(float temperature, float humidity)
    {
        BiomeGenerationData closestBiome = null;
        float closestDistance = float.MaxValue;

        foreach (var biome in biomes)
        {
            float avgTemp = (biome.temperature + biome.temperatureMax) / 2;
            float avgHumidity = (biome.humidity + biome.humidityMax) / 2;

            float tempDifference = Mathf.Abs(avgTemp - temperature);
            float humidityDifference = Mathf.Abs(avgHumidity - humidity);
            float distance = tempDifference + humidityDifference;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBiome = biome;
            }
        }

        return closestBiome;
    }

    public List<Vector2Int> GetPoints()
    {
        int minDistance = 40;
        List<Vector2Int> generatedPoints = new List<Vector2Int>();

        System.Random random = new System.Random(123);

        while (generatedPoints.Count < 5)
        {
            int randomX = random.Next(0, worldGenerationData.worldSize);
            int randomY = random.Next(0, worldGenerationData.worldSize);
            Vector2Int randomPoint = new Vector2Int(randomX, randomY);

            bool isFarEnough = true;
            foreach (Vector2 point in generatedPoints)
            {
                if (Vector2.Distance(randomPoint, point) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            if (isFarEnough)
            {
                generatedPoints.Add(randomPoint);
            }
        }

        return generatedPoints;
    }

    // private Biomes GetBiomeForPosition(Vector2Int pos)
    // {
    //     float temperatureNoiseValue = MyNoise.OctavePerlin(pos.x, pos.y, temperatureNoiseSettings);
    //     float humidityNoiseValue = MyNoise.OctavePerlin(pos.x, pos.y, humidityNoiseSettings);

    //     BiomeGenerationData biomeGenerationData = SelectBiome(temperatureNoiseValue, humidityNoiseValue);

    //     return biomeGenerationData.biome;
    // }

    public Biomes GetBiome(int index)
    {
        if (index == 0)
        {
            return Biomes.GRASSLAND;
        }
        if (index == 1)
        {
            return Biomes.FOREST;
        }
        if (index == 2)
        {
            return Biomes.JUNGLE;
        }
        if (index == 3)
        {
            return Biomes.DESERT;
        }
        if (index == 4)
        {
            return Biomes.SNOW;
        }

        return Biomes.WATER;
    }

    public int FindClosestPointIndex(List<Vector2Int> points, Vector2Int position)
    {
        if (points.Count == 0)
        {
            return -1;
        }

        int closestIndex = 0;
        float closestDistance = Vector2.Distance(position, points[0]);

        for (int i = 1; i < points.Count; i++)
        {
            float distance = Vector2.Distance(position, points[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private Dictionary<Vector2Int, Biomes> GenerateWorld()
    {
        Dictionary<Vector2Int, Biomes> world = new Dictionary<Vector2Int, Biomes>();
        List<Vector2Int> points = GetPoints();

        for (int x = 0; x < worldGenerationData.worldSize; x++)
        {
            for (int y = 0; y < worldGenerationData.worldSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                float noiseValue = MyNoise.OctavePerlin(x, y, noiseSettings);
                Biomes biome = Biomes.WATER;

                if (noiseValue > worldGenerationData.terrainThreshold)
                {
                    // biome = GetBiomeForPosition(pos);
                    Vector2Int r = VoronoiEdgeDistortion.Get2DTurbulence(pos, distortionNoiseSettings);
                    biome = GetBiome(FindClosestPointIndex(points, pos + r));
                }

                world[pos] = biome;
            }
        }

        return world;
    }
}
