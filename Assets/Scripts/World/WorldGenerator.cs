using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public NoiseSettings noiseSettings;
    private List<BiomeGenerator> biomeGenerators = new List<BiomeGenerator>();

    public Tilemap tilemap;
    public Tilemap tilemapDecoration;

    private Dictionary<Vector2Int, Biomes> biomes;
    private Dictionary<Vector2Int, ChunkData> chunksData;

    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    public Transform player;
    private Vector2Int lastPlayerChunkPosition = new Vector2Int(int.MinValue, int.MinValue);
    private int seed = 123123;
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


                    if (distanceSqr < closestDistanceSqr && distanceSqr < 15 * chunkSize)
                    {
                        closestDistanceSqr = distanceSqr;
                        closestBiome = biomeCenter.Value;
                    }
                }
                Vector2Int randomPosInChunk = new Vector2Int(random.Next(0, chunkSize), random.Next(0, chunkSize));
                int randomSeed = random.Next(0, int.MaxValue);
                newChunkData[new Vector2Int(x, y)] = new ChunkData(closestBiome, new Vector2Int(x * chunkSize, y * chunkSize) + randomPosInChunk, randomSeed);
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

        for (int i = 0; i < biomeGenerators.Count * 2; i++) // @TEMP 2, because we want to generate each biom twice
        {
            int index = i % biomeGenerators.Count;
            BiomeGenerator biomeGenerator = biomeGenerators[index];

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

        ChunkData chunkData = chunksData[chunkPos];

        System.Random chunkRandom = new System.Random(chunkData.randomSeed);

        Chunk chunk = new Chunk(chunkPos);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector2Int tilePosInChunk = new Vector2Int(x, y);
                Vector2Int fullTilePos = new Vector2Int(chunkPos.x * chunkSize, chunkPos.y * chunkSize) + tilePosInChunk;
                float noiseValue = MyNoise.OctavePerlin(fullTilePos.x, fullTilePos.y, noiseSettings);
                TileBase tile = waterTile;
                TileBase decorationTile = null;
                GameObject worldObject = null;

                if (noiseValue > worldGenerationData.terrainThreshold || (fullTilePos.x > -50 && fullTilePos.x < 50 && fullTilePos.y > -50 && fullTilePos.y < 50))
                {
                    Biomes biome = WorldGeneratorHelper.FindClosestBiome(worldGenerationData, chunksData, fullTilePos);
                    BiomeGenerator biomeGenerator = WorldGeneratorHelper.GetBiomeGeneratorByBiome(biomeGenerators, biome);

                    if (biomeGenerator)
                    {
                        var tileData = biomeGenerator.GetTile(fullTilePos, ref chunkRandom);
                        tile = tileData.Item1;
                        decorationTile = tileData.Item2;
                        worldObject = tileData.Item3;
                    }
                }

                chunk.tiles[tilePosInChunk] = tile;
                chunk.decorationTiles[tilePosInChunk] = decorationTile;
                if (worldObject != null)
                {
                    chunk.objects[tilePosInChunk] = worldObject;
                }
            }
        }

        return chunk;
    }

    private void RenderChunk(Chunk chunk)
    {
        Dictionary<Vector2Int, TileBase> tiles = chunk.tiles;
        Dictionary<Vector2Int, TileBase> decorationTiles = chunk.decorationTiles;
        Dictionary<Vector2Int, GameObject> objects = chunk.objects;
        Vector2Int chunkPos = chunk.pos;

        foreach (var vkp in tiles)
        {
            Vector2Int tilePos = WorldGeneratorHelper.ChunkTilePositionToTilemap(worldGenerationData, chunkPos, vkp.Key);
            tilemap.SetTile((Vector3Int)tilePos, vkp.Value);
            tilemapDecoration.SetTile((Vector3Int)tilePos, decorationTiles[vkp.Key]);
            if (objects.ContainsKey(vkp.Key))
            {
                GameObject newObject = Instantiate(objects[vkp.Key], (Vector3Int)tilePos, Quaternion.identity);
                chunk.objects[vkp.Key] = newObject;
            }
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
                tilemapDecoration.SetTile(new Vector3Int((chunkPos.x * chunkSize) + x, (chunkPos.y * chunkSize) + y, 0), null);
            }
        }

        foreach (Vector2Int key in chunk.objects.Keys.ToList())
        {
            if (chunk.objects[key] != null)
            {
                Destroy(chunk.objects[key]);
                chunk.objects[key] = null;
            }
        }
        chunks.Remove(chunkPos);
    }


}
