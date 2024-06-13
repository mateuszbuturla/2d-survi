using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUpscaler
{
    private int chunkSize = 100;

    public Dictionary<Vector2Int, TileBase> UpscaleTiles(Dictionary<Vector2Int, TileBase> originalTiles, int originalSize, int newSize)
    {
        int factor = newSize / originalSize;
        ConcurrentDictionary<Vector2Int, TileBase> upscaledTiles = new ConcurrentDictionary<Vector2Int, TileBase>();

        List<List<Vector2Int>> chunks = GetChunks(originalTiles.Keys, chunkSize);

        List<Task> tasks = new List<Task>();
        foreach (var chunk in chunks)
        {
            var task = Task.Run(() =>
            {
                foreach (var pos in chunk)
                {
                    TileBase originalTile = originalTiles[pos];
                    UpscaleTileChunk(upscaledTiles, pos, originalTile, factor);
                }
            });
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());

        return new Dictionary<Vector2Int, TileBase>(upscaledTiles);
    }

    private List<List<Vector2Int>> GetChunks(IEnumerable<Vector2Int> keys, int chunkSize)
    {
        List<List<Vector2Int>> chunks = new List<List<Vector2Int>>();
        List<Vector2Int> chunk = new List<Vector2Int>();

        foreach (var key in keys)
        {
            chunk.Add(key);
            if (chunk.Count >= chunkSize)
            {
                chunks.Add(chunk);
                chunk = new List<Vector2Int>();
            }
        }

        if (chunk.Count > 0)
        {
            chunks.Add(chunk);
        }

        return chunks;
    }

    private void UpscaleTileChunk(ConcurrentDictionary<Vector2Int, TileBase> upscaledTiles, Vector2Int originalPos, TileBase originalTile, int factor)
    {
        for (int dx = 0; dx < factor; dx++)
        {
            for (int dy = 0; dy < factor; dy++)
            {
                Vector2Int newPos = new Vector2Int(originalPos.x * factor + dx, originalPos.y * factor + dy);
                upscaledTiles[newPos] = originalTile;
            }
        }
    }
}