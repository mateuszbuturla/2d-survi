using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestChunk
{
    public Vector2Int pos;

    public TestChunk(Vector2Int pos)
    {
        this.pos = pos;
    }
}

public class TestGenerator : MonoBehaviour
{

    public NoiseSettings noiseSettings;
    public NoiseSettings noiseSettings2;
    public int renderDistance = 2;
    public int chunkSize = 16;
    public float noiseScale = 0.1f;
    public Tilemap tilemap;
    public TileBase[] tiles;

    public Transform player;
    private Dictionary<Vector2Int, TestChunk> chunks = new Dictionary<Vector2Int, TestChunk>();
    public Vector2Int lastPlayerChunkPos = new Vector2Int(int.MinValue, int.MinValue);

    void Start()
    {
        UpdateChunksAroundPlayer();
    }

    void Update()
    {
        UpdateChunksAroundPlayer();
    }

    void UpdateChunksAroundPlayer()
    {
        Vector3Int playerPos = Vector3Int.FloorToInt(player.position);
        Vector3Int playerCellPos = tilemap.WorldToCell(playerPos);
        int playerChunkX = Mathf.FloorToInt(playerCellPos.x / (float)chunkSize);
        int playerChunkY = Mathf.FloorToInt(playerCellPos.y / (float)chunkSize);
        Vector2Int newPlayerChunkPosition = new Vector2Int(playerChunkX, playerChunkY);

        if (lastPlayerChunkPos != newPlayerChunkPosition)
        {
            List<Vector2Int> chunksToRemove = GetChunksToRemove(newPlayerChunkPosition);

            foreach (var chunkToRemove in chunksToRemove)
            {
                RemoveChunk(chunks[chunkToRemove]);
            }

            List<Vector2Int> chunksToCreate = GetChunksToCreate(newPlayerChunkPosition);

            foreach (var chunkToCreate in chunksToCreate)
            {
                CreateChunk(chunkToCreate);
            }

            lastPlayerChunkPos = newPlayerChunkPosition;
        }
    }

    private List<Vector2Int> GetChunksToCreate(Vector2Int newPlayerPos)
    {
        List<Vector2Int> chunksToCreate = new List<Vector2Int>();

        for (int x = newPlayerPos.x - renderDistance; x <= newPlayerPos.x + renderDistance; x++)
        {
            for (int y = newPlayerPos.y - renderDistance; y <= newPlayerPos.y + renderDistance; y++)
            {
                Vector2Int chunkPos = new Vector2Int(x, y);
                if (!chunks.ContainsKey(chunkPos))
                {
                    chunksToCreate.Add(chunkPos);
                }
            }
        }

        return chunksToCreate;
    }

    private List<Vector2Int> GetChunksToRemove(Vector2Int newPlayerPos)
    {
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

    void CreateChunk(Vector2Int chunkPos)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float perlinValue = MyNoise.OctavePerlin((chunkPos.x * chunkSize) + x, (chunkPos.y * chunkSize) + y, noiseSettings);
                TileBase tile = tiles[0];

                if (perlinValue > 0.5f)
                {
                    tile = tiles[1];
                }

                tilemap.SetTile(new Vector3Int((chunkPos.x * chunkSize) + x, (chunkPos.y * chunkSize) + y, 0), tile);
            }
        }
        chunks[chunkPos] = new TestChunk(chunkPos);
    }

    void RemoveChunk(TestChunk chunk)
    {
        Vector2Int chunkPos = chunk.pos;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                tilemap.SetTile(new Vector3Int((chunkPos.x * chunkSize) + x, (chunkPos.y * chunkSize) + y, 0), null);
            }
        }
        chunks.Remove(chunkPos);
    }

    TileBase GetTileForValue(float value)
    {
        if (value < 0.5f)
            return tiles[0];
        else
            return tiles[1];
    }
}