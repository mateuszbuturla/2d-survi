using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public NoiseSettings noiseSettings;

    public Tilemap tilemap;

    public TileBase waterTile;
    public TileBase grassTile;

    private void ClearTilemap()
    {
        tilemap.ClearAllTiles();
        tilemap.RefreshAllTiles();
    }

    public void GenerateWorld()
    {
        ClearTilemap();
        for (int x = 0; x < worldGenerationData.worldSize; x++)
        {
            for (int y = 0; y < worldGenerationData.worldSize; y++)
            {
                float noiseValue = MyNoise.OctavePerlin(x, y, noiseSettings);
                TileBase tile = waterTile;

                if (noiseValue > worldGenerationData.terrainThreshold)
                {
                    tile = grassTile;
                }


                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
