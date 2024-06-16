using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public NoiseSettings noiseSettings;

    public Tilemap tilemap;

    public TileBase waterTile;
    public TileBase forestTile;
    public TileBase grasslandTile;
    public TileBase snowTile;
    public TileBase desertTile;
    public TileBase jungleTile;

    public TileBase corruptedTile;
    public TileBase wastelandTile;
    public TileBase holyTile;
    public TileBase mechaTile;

    private Dictionary<Vector2Int, Biomes> biomes;

    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    public Transform player;
    public Vector2Int lastPlayerChunkPosition = new Vector2Int(int.MinValue, int.MinValue);
    private int seed = 123;


    System.Random random;
    void Start()
    {
        random = new System.Random(seed);
        biomes = GetBiomesCenterPoints();

        UpdateChunksAroundPlayer();
    }

    void Update()
    {
        UpdateChunksAroundPlayer();
    }

    private Dictionary<Vector2Int, Biomes> GetBiomesCenterPoints()
    {
        Dictionary<Vector2Int, Biomes> generatedPoints = new Dictionary<Vector2Int, Biomes>();

        generatedPoints[new Vector2Int(0, 0)] = Biomes.GRASSLAND;

        while (generatedPoints.Count < 9)
        {
            float angle = (float)(360 / 4 * generatedPoints.Count);
            int distance = 150;

            if (generatedPoints.Count > 4)
            {
                distance = 300;
            }

            int x = (int)(distance * Mathf.Cos(angle));
            int y = (int)(distance * Mathf.Sin(angle));
            Vector2Int randomPoint = new Vector2Int(x, y);

            bool isFarEnough = true;

            if (isFarEnough)
            {
                Biomes biome = GetBiome(generatedPoints.Count);
                generatedPoints[randomPoint] = biome;
            }
        }



        Debug.Log(generatedPoints.Count);

        return generatedPoints;
    }

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
        if (index == 5)
        {
            return Biomes.CORRUPTED;
        }
        if (index == 6)
        {
            return Biomes.HOLY;
        }
        if (index == 7)
        {
            return Biomes.MECHA;
        }
        if (index == 8)
        {
            return Biomes.WASTELAND;
        }

        return Biomes.WATER;
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
                    biome = WorldGeneratorHelper.FindClosestBiome(worldGenerationData, biomes, fullTilePos);
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
            TileBase tile = GetTile(vkp.Value);
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
        if (biome == Biomes.CORRUPTED)
        {
            return corruptedTile;
        }
        if (biome == Biomes.HOLY)
        {
            return holyTile;
        }
        if (biome == Biomes.WASTELAND)
        {
            return wastelandTile;
        }
        if (biome == Biomes.MECHA)
        {
            return mechaTile;
        }

        return waterTile;
    }
}
