using System.Collections.Generic;
using UnityEngine;

public static class WorldGeneratorHelper
{
    public static Vector2Int GetPlayerChunkPos(WorldGenerationData worldGenerationData, Vector2Int playerCellPos)
    {
        int chunkSize = worldGenerationData.chunkSize;

        int playerChunkX = Mathf.FloorToInt(playerCellPos.x / (float)chunkSize);
        int playerChunkY = Mathf.FloorToInt(playerCellPos.y / (float)chunkSize);

        return new Vector2Int(playerChunkX, playerChunkY);
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
}
