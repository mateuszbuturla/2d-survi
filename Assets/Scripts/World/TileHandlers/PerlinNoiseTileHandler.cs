using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseTileHandler : TileHandler
{
    public NoiseSettings settings;
    [Range(0, 1)]
    public float threshold;
    public TestData data;

    public TileBase testTile;
    public TileBase testTile2;
    public TileBase testTile3;
    public TileBase testTile4;

    protected override TileBase TryHandling(Vector2Int pos, System.Random random)
    {
        float value = MyNoise.OctavePerlin(pos.x, pos.y, settings);
        bool valueRight = MyNoise.OctavePerlin(pos.x + 1, pos.y, settings) > threshold;
        bool valueLeft = MyNoise.OctavePerlin(pos.x - 1, pos.y, settings) > threshold;
        bool valueTop = MyNoise.OctavePerlin(pos.x, pos.y + 1, settings) > threshold;
        bool valueBottom = MyNoise.OctavePerlin(pos.x, pos.y - 1, settings) > threshold;
        bool valueBottomLeft = MyNoise.OctavePerlin(pos.x - 1, pos.y - 1, settings) > threshold;
        bool valueBottomRight = MyNoise.OctavePerlin(pos.x + 1, pos.y - 1, settings) > threshold;
        bool valueTopLeft = MyNoise.OctavePerlin(pos.x - 1, pos.y + 1, settings) > threshold;
        bool valueTopRight = MyNoise.OctavePerlin(pos.x + 1, pos.y + 1, settings) > threshold;

        if (value > threshold)
        {
            if (valueTop && valueRight && !valueBottom && !valueLeft)
            {
                return data.tiles[6];
            }
            else if (valueTop && !valueRight && !valueBottom && valueLeft)
            {
                return data.tiles[8];
            }
            else if (!valueTop && valueRight && valueBottom && !valueLeft)
            {
                return data.tiles[0];
            }
            else if (!valueTop && !valueRight && valueBottom && valueLeft)
            {
                return data.tiles[2];
            }
            else if (valueTop && valueRight && valueBottom && valueLeft && !valueBottomLeft)
            {
                return testTile;
            }
            else if (valueTop && valueRight && valueBottom && valueLeft && !valueBottomRight)
            {
                return testTile2;
            }
            else if (valueTop && valueRight && valueBottom && valueLeft && !valueTopLeft)
            {
                return testTile3;
            }
            else if (valueTop && valueRight && valueBottom && valueLeft && !valueTopRight)
            {
                return testTile4;
            }
            else if (valueRight && !valueLeft)
            {
                return data.tiles[3];
            }
            else if (!valueRight && valueLeft)
            {
                return data.tiles[5];
            }
            else if (!valueTop && valueBottom)
            {
                return data.tiles[1];
            }
            else if (valueTop && !valueBottom)
            {
                return data.tiles[7];
            }

            return data.tiles[4];
        }
        return null;
    }
}
