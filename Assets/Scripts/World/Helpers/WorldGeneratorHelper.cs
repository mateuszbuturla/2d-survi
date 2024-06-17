using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WorldGeneratorHelper
{
    public static Vector2Int PosToChunk(WorldGenerationData worldGenerationData, Vector2Int pos)
    {
        int chunkSize = worldGenerationData.chunkSize;

        int posChunkX = Mathf.FloorToInt(pos.x / (float)chunkSize);
        int posChunkY = Mathf.FloorToInt(pos.y / (float)chunkSize);

        return new Vector2Int(posChunkX, posChunkY);
    }

    public static List<Vector2Int> GetChunksAround(Vector2Int newPlayerPos, int range)
    {
        List<Vector2Int> chunks = new List<Vector2Int>();

        for (int x = newPlayerPos.x - range; x <= newPlayerPos.x + range; x++)
        {
            for (int y = newPlayerPos.y - range; y <= newPlayerPos.y + range; y++)
            {
                Vector2Int chunkPos = new Vector2Int(x, y);
                chunks.Add(chunkPos);
            }
        }

        return chunks;
    }

    public static Vector2Int ChunkTilePositionToTilemap(WorldGenerationData worldGenerationData, Vector2Int chunkPos, Vector2Int pointPos)
    {
        int chunkSize = worldGenerationData.chunkSize;

        return new Vector2Int(chunkPos.x * chunkSize, chunkPos.y * chunkSize) + pointPos;
    }

    public static List<Vector2Int> GetChunksToRemove(WorldGenerationData worldGenerationData, Dictionary<Vector2Int, Chunk> chunks, Vector2Int newPlayerPos)
    {
        int renderDistance = worldGenerationData.renderDistance;
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (var chunk in chunks.Keys)
        {
            int distanceX = Mathf.Abs(chunk.x - newPlayerPos.x);
            int distanceY = Mathf.Abs(chunk.y - newPlayerPos.y);
            if (distanceX > renderDistance || distanceY > renderDistance)
            {
                chunksToRemove.Add(chunk);
            }
        }

        return chunksToRemove;
    }

    public static Vector2Int GetChunkCenterPoint(WorldGenerationData worldGenerationData)
    {
        return new Vector2Int(worldGenerationData.chunkSize / 2, worldGenerationData.chunkSize / 2);
    }

    public static Biomes FindClosestBiome(WorldGenerationData worldGenerationData, Dictionary<Vector2Int, ChunkData> chunksData, Vector2Int position)
    {
        Vector2Int closestPosition = Vector2Int.zero;
        float closestDistanceSqr = Mathf.Infinity;

        Vector2Int chunk = PosToChunk(worldGenerationData, position);

        Dictionary<Vector2Int, Biomes> filtredChunks = new Dictionary<Vector2Int, Biomes>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (chunksData.ContainsKey(chunk + new Vector2Int(x, y)))
                {
                    Vector2Int p = chunk + new Vector2Int(x, y);
                    filtredChunks[chunksData[p].biomePoint] = chunksData[p].biome;
                }
            }
        }

        foreach (KeyValuePair<Vector2Int, Biomes> biome in filtredChunks)
        {
            Vector2Int diff = biome.Key - position;
            float distanceSqr = diff.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPosition = biome.Key;
            }
        }

        return filtredChunks[closestPosition];
    }

    public static TileBase GetTile(List<BiomeGenerator> biomeGenerators, TileBase waterTile, Biomes biome)
    {
        BiomeGenerator biomeGenerator = biomeGenerators.Find(b => b.biome == biome);

        if (biomeGenerator != null)
        {
            return biomeGenerator.baseTile;
        }

        return waterTile;
    }

    public static BiomeGenerator GetSpawnBiome(List<BiomeGenerator> biomeGenerators)
    {
        BiomeGenerator biomeGenerator = biomeGenerators.Find(b => b.biomeType == BiomeType.SPAWN);

        return biomeGenerator;
    }

    public static BiomeGenerator GetBiomeGeneratorByBiome(List<BiomeGenerator> biomeGenerators, Biomes biome)
    {
        BiomeGenerator biomeGenerator = biomeGenerators.Find(b => b.biome == biome);

        return biomeGenerator;
    }
}
