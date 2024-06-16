using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public NoiseSettings noiseSettings;
    public List<BiomeGenerator> biomeGenerators = new List<BiomeGenerator>();

    public Tilemap tilemap;

    private Dictionary<Vector2Int, Biomes> biomes;
    private Dictionary<Vector2Int, ChunkData> chunksData;

    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    public Transform player;
    public Vector2Int lastPlayerChunkPosition = new Vector2Int(int.MinValue, int.MinValue);
    private int seed = 123;
    public TileBase waterTile;

    System.Random random;
    void Start()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            BiomeGenerator biomeGenerator = child.GetComponent<BiomeGenerator>();
            biomeGenerators.Add(biomeGenerator);
        }

        random = new System.Random(seed);
        biomes = GetBiomesCenterPoints();
        chunksData = GenerateChunksData();

        UpdateChunksAroundPlayer();
    }


    void Update()
    {
        UpdateChunksAroundPlayer();
    }

    private Dictionary<Vector2Int, ChunkData> GenerateChunksData()
    {
        int chunkSize = worldGenerationData.chunkSize;
        int halfSize = worldGenerationData.worldSizeInChunk / 2;

        Dictionary<Vector2Int, ChunkData> newChunkData = new Dictionary<Vector2Int, ChunkData>();

        for (int x = -halfSize; x < halfSize; x++)
        {
            for (int y = -halfSize; y < halfSize; y++)
            {
                Biomes closestBiome = Biomes.WATER;
                float closestDistanceSqr = Mathf.Infinity;

                foreach (KeyValuePair<Vector2Int, Biomes> biomeCenter in biomes)
                {
                    float distanceSqr = Vector2.Distance(new Vector2Int(x * chunkSize, y * chunkSize), biomeCenter.Key);


                    if (distanceSqr < closestDistanceSqr && distanceSqr < 20 * chunkSize)
                    {
                        closestDistanceSqr = distanceSqr;
                        closestBiome = biomeCenter.Value;
                    }
                }
                Vector2Int randomPosInChunk = new Vector2Int(random.Next(0, chunkSize), random.Next(0, chunkSize));
                newChunkData[new Vector2Int(x, y)] = new ChunkData(closestBiome, new Vector2Int(x * chunkSize, y * chunkSize) + randomPosInChunk);
            }
        }

        return newChunkData;
    }

    private Dictionary<Vector2Int, Biomes> GetBiomesCenterPoints()
    {
        Dictionary<Vector2Int, Biomes> generatedPoints = new Dictionary<Vector2Int, Biomes>();

        BiomeGenerator defaultBiome = WorldGeneratorHelper.GetSpawnBiome(biomeGenerators);

        if (defaultBiome == null)
        {
            Debug.LogError("Spawn biome couldn't be found");
            return new Dictionary<Vector2Int, Biomes>();
        }

        generatedPoints[new Vector2Int(0, 0)] = defaultBiome.biome;

        for (int i = 0; i < biomeGenerators.Count; i++)
        {
            BiomeGenerator biomeGenerator = biomeGenerators[i];

            if (biomeGenerator == defaultBiome)
            {
                continue;
            }

            int distance = 300;
            float angle = random.Next(0, 360);

            if (biomeGenerator.biomeType == BiomeType.OUTER)
            {
                distance = 600;
            }

            int x = (int)(distance * Mathf.Cos(angle));
            int y = (int)(distance * Mathf.Sin(angle));

            Vector2Int randomPoint = new Vector2Int(x, y);
            generatedPoints[randomPoint] = biomeGenerator.biome;
        }

        // Add water around world
        int half = worldGenerationData.worldSizeInChunk / 2;
        for (int i = -half; i < half; i++)
        {
            generatedPoints[new Vector2Int(i * worldGenerationData.chunkSize, half * worldGenerationData.chunkSize)] = Biomes.WATER;
            generatedPoints[new Vector2Int(i * worldGenerationData.chunkSize, -half * worldGenerationData.chunkSize)] = Biomes.WATER;

            generatedPoints[new Vector2Int(half * worldGenerationData.chunkSize, i * worldGenerationData.chunkSize)] = Biomes.WATER;
            generatedPoints[new Vector2Int(-half * worldGenerationData.chunkSize, i * worldGenerationData.chunkSize)] = Biomes.WATER;
        }

        return generatedPoints;
    }

    public void UpdateChunksAroundPlayer()
    {
        int chunkCount = worldGenerationData.worldSizeInChunk;
        Vector3 playerPos = player.position;
        Vector2Int playerCellPos = (Vector2Int)tilemap.WorldToCell(playerPos);
        Vector2Int playerChunkPos = WorldGeneratorHelper.PosToChunk(worldGenerationData, playerCellPos);

        if (playerChunkPos != lastPlayerChunkPosition)
        {
            List<Vector2Int> chunksToRemove = WorldGeneratorHelper.GetChunksToRemove(worldGenerationData, chunks, playerChunkPos);

            foreach (var chunkToRemove in chunksToRemove)
            {
                RemoveChunk(chunks[chunkToRemove]);
            }

            List<Vector2Int> chunksToGenerate = WorldGeneratorHelper.GetChunksAround(playerChunkPos, worldGenerationData.renderDistance);

            int halfSize = chunkCount / 2;

            foreach (var chunkToGenerate in chunksToGenerate)
            {
                if (chunkToGenerate.x >= -halfSize &&
                chunkToGenerate.x <= halfSize &&
                chunkToGenerate.y >= -halfSize &&
                chunkToGenerate.y <= halfSize &&
                !chunks.ContainsKey(chunkToGenerate))
                {
                    Chunk newChunk = GenerateChunk(chunkToGenerate);

                    chunks[chunkToGenerate] = newChunk;
                    RenderChunk(newChunk);
                }
            }

            lastPlayerChunkPosition = playerChunkPos;
        }
    }

    private Chunk GenerateChunk(Vector2Int chunkPos)
    {
        int chunkSize = worldGenerationData.chunkSize;

        Chunk chunk = new Chunk(chunkPos);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector2Int tilePosInChunk = new Vector2Int(x, y);
                Vector2Int fullTilePos = new Vector2Int(chunkPos.x * chunkSize, chunkPos.y * chunkSize) + tilePosInChunk;
                float noiseValue = MyNoise.OctavePerlin(fullTilePos.x, fullTilePos.y, noiseSettings);
                Biomes biome = Biomes.WATER;

                if (noiseValue > worldGenerationData.terrainThreshold || (fullTilePos.x > -50 && fullTilePos.x < 50 && fullTilePos.y > -50 && fullTilePos.y < 50))
                {
                    biome = WorldGeneratorHelper.FindClosestBiome(worldGenerationData, chunksData, fullTilePos);
                }

                chunk.tiles[tilePosInChunk] = biome;
            }
        }

        return chunk;
    }

    private void RenderChunk(Chunk chunk)
    {
        Dictionary<Vector2Int, Biomes> tiles = chunk.tiles;
        Vector2Int chunkPos = chunk.pos;

        foreach (var vkp in tiles)
        {
            Vector2Int tilePos = WorldGeneratorHelper.ChunkTilePositionToTilemap(worldGenerationData, chunkPos, vkp.Key);
            TileBase tile = WorldGeneratorHelper.GetTile(biomeGenerators, waterTile, vkp.Value);
            tilemap.SetTile((Vector3Int)tilePos, tile);
        }
    }

    private void RemoveChunk(Chunk chunk)
    {
        Vector2Int chunkPos = chunk.pos;
        int chunkSize = worldGenerationData.chunkSize;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                tilemap.SetTile(new Vector3Int((chunkPos.x * chunkSize) + x, (chunkPos.y * chunkSize) + y, 0), null);
            }
        }
        chunks.Remove(chunkPos);
    }


}
