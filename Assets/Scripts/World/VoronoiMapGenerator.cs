using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class VoronoiMapGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public VoronoiDistortionData voronoiDistortionData;

    public Tilemap tilemap;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

    private List<Vector3Int> points;

    private List<Vector3Int> connected = new List<Vector3Int>();

    public GenerateBiome roadGenerator;

    public List<Biome> biomes;

    public GameObject tree;

    private Dictionary<Vector2Int, List<Biome>> biomeGrid;

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        biomes = GenerateBiomeBase();
        CreateBiomeGrid();
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Execution Time GenerateBiomeBase: {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Reset();
        stopwatch.Start();
        GenerateMap();
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Execution Time GenerateMap: {stopwatch.ElapsedMilliseconds} ms");
    }

    void GenerateMap()
    {
        for (int x = 0; x < worldGenerationData.worldWidth; x++)
        {
            for (int y = 0; y < worldGenerationData.worldHeight; y++)
            {
                Biome closestBiome = FindClosestBiome(new Vector3Int(x, y, 0));
                BiomeData biomeGenerator = biomeGeneratorsData.Find(bg => bg.biomeType == closestBiome.biomeType);

                if (closestBiome != null)
                {
                    closestBiome.biomePoints.Add(new Vector2Int(x, y));
                }

                biomeGenerator.biomeGenerator.GenerateTile(new Vector3Int(x, y, 0));
            }
        }

        foreach (Biome biome in biomes)
        {
            roadGenerator.GenerateTile(new Vector3Int(biome.mainBiomePoint.x, biome.mainBiomePoint.y, 0));
        }
    }

    public BiomeData GetBiomeGeneratorByBiomeType(BiomeType biomeType)
    {
        return biomeGeneratorsData.Find(x => x.biomeType == biomeType);
    }

    public Biome FindClosestBiome(Vector3Int position)
    {
        Biome closestBiome = null;
        float closestDistance = float.MaxValue;
        int segmentSize = worldGenerationData.worldWidth / worldGenerationData.segmentCount;

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

    public List<Biome> GenerateBiomeBase()
    {
        List<Biome> generatedBiomes = new List<Biome>();

        int segmentSizeX = worldGenerationData.worldWidth / worldGenerationData.segmentCount;
        int segmentSizeY = worldGenerationData.worldHeight / worldGenerationData.segmentCount;

        for (int xSegment = 0; xSegment < worldGenerationData.segmentCount; xSegment++)
        {
            for (int ySegment = 0; ySegment < worldGenerationData.segmentCount; ySegment++)
            {
                int startX = xSegment * segmentSizeX;
                int endX = (xSegment + 1) * segmentSizeX;
                int startY = ySegment * segmentSizeY;
                int endY = (ySegment + 1) * segmentSizeY;

                int mapCenter = (int)(worldGenerationData.segmentCount / 2) - 1;

                BiomeType biomeType;
                if (xSegment <= worldGenerationData.borderThickness || xSegment >= worldGenerationData.segmentCount - worldGenerationData.borderThickness || ySegment <= worldGenerationData.borderThickness || ySegment >= worldGenerationData.segmentCount - worldGenerationData.borderThickness)
                {
                    biomeType = BiomeType.WATER;
                }
                else if (xSegment >= mapCenter && xSegment < mapCenter + worldGenerationData.startBiomeMinSize && ySegment >= mapCenter && ySegment < mapCenter + worldGenerationData.startBiomeMinSize)
                {
                    biomeType = worldGenerationData.startBiome;
                }
                else
                {
                    int randomBiome = Random.Range(0, biomeGeneratorsData.Count);
                    biomeType = biomeGeneratorsData[randomBiome].biomeType;
                }

                int x = Random.Range(startX, endX);
                int y = Random.Range(startY, endY);

                Vector2Int baseBiomePoint = new Vector2Int(x, y);

                generatedBiomes.Add(new Biome(biomeType, baseBiomePoint));
            }
        }

        return generatedBiomes;
    }

    private void CreateBiomeGrid()
    {
        biomeGrid = new Dictionary<Vector2Int, List<Biome>>();

        int segmentSize = worldGenerationData.worldHeight / worldGenerationData.segmentCount;

        foreach (Biome biome in biomes)
        {
            Vector2Int gridPos = new Vector2Int(biome.mainBiomePoint.x / segmentSize, biome.mainBiomePoint.y / segmentSize);

            if (!biomeGrid.ContainsKey(gridPos))
            {
                biomeGrid[gridPos] = new List<Biome>();
            }

            biomeGrid[gridPos].Add(biome);
        }
    }
}