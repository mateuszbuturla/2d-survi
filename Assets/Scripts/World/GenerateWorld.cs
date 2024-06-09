using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateWorld : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public TileBase[] tiles;
    public Tilemap tilemap;

    private int width;
    private int height;
    private float scale;

    void Start() {
        this.width = this.worldGenerationData.worldWidth;
        this.height = this.worldGenerationData.worldHeight;
        this.scale = this.worldGenerationData.worldScale;
        GenerateTilemap();
    }

    void GenerateTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                int tileIndex = Mathf.Clamp(Mathf.FloorToInt(sample * tiles.Length), 0, tiles.Length - 1);
                TileBase tile = tiles[tileIndex];
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
