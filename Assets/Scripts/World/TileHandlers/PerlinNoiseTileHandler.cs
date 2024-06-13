using UnityEngine;

public class PerlinNoiseTileHandler : TileHandler
{
    public PerlinNoiseTileHandlerData settings;
    [Range(0, 1)]
    public float threshold;

    protected override bool TryHandling(Vector2Int pos)
    {
        float value = Mathf.PerlinNoise(pos.x * settings.scale, pos.y * settings.scale);

        if (value > threshold)
        {
            return true;
        }
        return false;
    }
}
