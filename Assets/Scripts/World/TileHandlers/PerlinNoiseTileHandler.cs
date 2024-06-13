using UnityEngine;

public class PerlinNoiseTileHandler : TileHandler
{
    public NoiseSettings settings;
    [Range(0, 1)]
    public float threshold;

    protected override bool TryHandling(Vector2Int pos)
    {
        float value = MyNoise.OctavePerlin(pos.x, pos.y, settings);

        if (value > threshold)
        {
            return true;
        }
        return false;
    }
}
