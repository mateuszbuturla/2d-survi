using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase baseTile;

    public TestData data;

    public NoiseSettings settings;
    [Range(0, 1)]
    public float threshold;

    void Start()
    {
        for (int x = -200; x < 200; x++)
        {
            for (int y = -200; y < 200; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), baseTile);
                int xBase = x + 200;
                int yBase = y + 200;

                float value = MyNoise.OctavePerlin(xBase, yBase, settings);
                bool valueRight = MyNoise.OctavePerlin(xBase + 1, yBase, settings) > threshold;
                bool valueLeft = MyNoise.OctavePerlin(xBase - 1, yBase, settings) > threshold;
                bool valueTop = MyNoise.OctavePerlin(xBase, yBase + 1, settings) > threshold;
                bool valueBottom = MyNoise.OctavePerlin(xBase, yBase - 1, settings) > threshold;

                if (value > threshold)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[4]);

                    if (valueTop && valueRight && !valueBottom && !valueLeft)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[6]);
                    }
                    else if (valueTop && !valueRight && !valueBottom && valueLeft)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[8]);
                    }
                    else if (!valueTop && valueRight && valueBottom && !valueLeft)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[0]);
                    }
                    else if (!valueTop && !valueRight && valueBottom && valueLeft)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[2]);
                    }
                    else if (valueRight && !valueLeft)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[3]);
                    }
                    else if (!valueRight && valueLeft)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[5]);
                    }
                    else if (!valueTop && valueBottom)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[1]);
                    }
                    else if (valueTop && !valueBottom)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), data.tiles[7]);
                    }

                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), baseTile);
                }
            }
        }
    }
}
