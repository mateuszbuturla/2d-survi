using System;
using System.Collections.Generic;
using UnityEngine;

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

    public static Biomes FindClosestBiome(WorldGenerationData worldGenerationData, Dictionary<Vector2Int, Biomes> biomes, Vector2Int position)
    {
        Vector2Int chunkCenterPoint = GetChunkCenterPoint(worldGenerationData);
        Vector2Int closestPosition = Vector2Int.zero;
        float closestDistanceSqr = Mathf.Infinity;


        foreach (KeyValuePair<Vector2Int, Biomes> biome in biomes)
        {
            // Vector2Int biomeCenterFullPos = ChunkTilePositionToTilemap(worldGenerationData, biome.Key, chunkCenterPoint);
            Vector2Int diff = biome.Key - position;
            float distanceSqr = diff.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPosition = biome.Key;
            }
        }

        return biomes[closestPosition];
    }
}
