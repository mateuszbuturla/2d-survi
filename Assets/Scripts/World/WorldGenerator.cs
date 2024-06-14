using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class WorldGenerator
{
    WorldGenerationData worldGenerationData;
    List<BiomeData> biomeGeneratorsData = new List<BiomeData>();
    VoronoiDistortionData voronoiDistortionData;
    System.Random random;
    public int seed;

    public WorldGenerator(List<BiomeData> biomeGeneratorsData, WorldGenerationData worldGenerationData, VoronoiDistortionData voronoiDistortionData, int seed)
    {
        this.biomeGeneratorsData = biomeGeneratorsData;
        this.worldGenerationData = worldGenerationData;
        this.voronoiDistortionData = voronoiDistortionData;
        this.seed = seed;

        random = new System.Random(seed);
    }

    public WorldDataDto GenerateWorld()
    {
        BiomesGenerator biomesGenerator = new BiomesGenerator(biomeGeneratorsData, worldGenerationData, ref random);
        Dictionary<Vector2Int, List<Biome>> biomeGrid = biomesGenerator.GenerateBiomes();

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        Dictionary<Vector2Int, TileBase> tiles = GenerateMap(biomeGrid);

        stopwatch.Stop();
        Debug.Log("GenerateMap: " + stopwatch.Elapsed.Milliseconds + " ms");

        EntitiesGenerator eg = new EntitiesGenerator(biomeGeneratorsData, biomeGrid, ref random);
        Dictionary<Vector2Int, GameObject> entities = eg.GenerateEntities();

        return new WorldDataDto(tiles, entities);
    }

    public Dictionary<Vector2Int, TileBase> UpscaleTiles(Dictionary<Vector2Int, TileBase> originalTiles, int originalSize, int newSize)
    {
        int factor = newSize / originalSize;
        Dictionary<Vector2Int, TileBase> upscaledTiles = new Dictionary<Vector2Int, TileBase>();

        foreach (var kvp in originalTiles)
        {
            Vector2Int originalPos = kvp.Key;
            TileBase originalTile = kvp.Value;

            for (int dx = 0; dx < factor; dx++)
            {
                for (int dy = 0; dy < factor; dy++)
                {
                    Vector2Int newPos = new Vector2Int(originalPos.x * factor + dx, originalPos.y * factor + dy);
                    upscaledTiles[newPos] = originalTile;
                }
            }
        }
        return upscaledTiles;
    }

    // Function for generating each tile based on the closest biome using voronoi diagram
    private Dictionary<Vector2Int, TileBase> GenerateMap(Dictionary<Vector2Int, List<Biome>> biomeGrid)
    {
        Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

        Dictionary<Vector2Int, Biome> simplifiedBiomes = new Dictionary<Vector2Int, Biome>();
        foreach (Vector2Int e in biomeGrid.Keys)
        {
            foreach (Biome biome in biomeGrid[e])
            {
                simplifiedBiomes[biome.segmentPos] = biome;
            }
        }

        int half = worldGenerationData.worldSize / 2;
        int chunkSize = 50;

        ConcurrentDictionary<Vector2Int, TileBase> concurrentTiles = new ConcurrentDictionary<Vector2Int, TileBase>();

        List<(int startX, int startY)> chunks = new List<(int startX, int startY)>();

        // Generate list of chunk start positions
        for (int x = -half; x < half; x += chunkSize)
        {
            for (int y = -half; y < half; y += chunkSize)
            {
                chunks.Add((x, y));
            }
        }

        Parallel.ForEach(chunks, chunk =>
        {
            int startX = chunk.startX;
            int startY = chunk.startY;

            for (int x = startX; x < startX + chunkSize && x < half; x++)
            {
                for (int y = startY; y < startY + chunkSize && y < half; y++)
                {
                    Biome closestBiome = FindClosestBiome(simplifiedBiomes, new Vector3Int(x, y, 0));
                    if (closestBiome != null)
                    {
                        BiomeData biomeGenerator = biomeGeneratorsData.Find(bg => bg.biomeType == closestBiome.biomeType);
                        Vector2Int pos = new Vector2Int(x, y);

                        lock (closestBiome)
                        {
                            closestBiome.biomePoints.Add(pos);
                        }

                        concurrentTiles[pos] = biomeGenerator.biomeGenerator.GetTile(pos);
                    }
                }
            }
        });

        // Convert ConcurrentDictionary back to Dictionary
        foreach (var kvp in concurrentTiles)
        {
            tiles[kvp.Key] = kvp.Value;
        }

        return tiles;
    }

    // Function for finding the closest biome (it uses voronoi diagram)
    private Biome FindClosestBiome(Dictionary<Vector2Int, Biome> simplifiedBiomes, Vector3Int position)
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
                if (simplifiedBiomes.ContainsKey(neighborPos))
                {
                    nearbyBiomes.Add(simplifiedBiomes[neighborPos]);
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
