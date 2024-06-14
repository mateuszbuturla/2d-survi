using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test2 : TileHandler
{
    public NoiseSettings settings;
    [Range(0, 1)]
    public float threshold;

    public TileBase tile;

    protected override TileBase TryHandling(Vector2Int pos, System.Random random)
    {
        float value = MyNoise.OctavePerlin(pos.x, pos.y, settings);

        if (value > threshold)
        {
            return tile;
        }
        return null;
    }
}