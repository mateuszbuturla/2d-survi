using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseTileHandler : TileHandler
{
    public NoiseSettings settings;
    [Range(0, 1)]
    public float threshold;

    public TileBase tile;

    protected override TileBase TryHandling(Vector2Int pos, ref System.Random random)
    {
        float value = MyNoise.OctavePerlin(pos.x, pos.y, settings);

        if (value > threshold)
        {
            return tile;
        }
        return null;
    }
}
