using System.Collections.Generic;
using UnityEngine;

public class TreeHandler : ObjectHandler
{
    [Range(0f, 100f)]
    public float threshold;

    public List<GameObject> prefabs;

    protected override GameObject TryHandling(Vector2Int pos, ref System.Random random)
    {
        float r = (float)(random.NextDouble() * 100);

        if (r >= threshold)
        {
            return null;
        }

        int objectIndex = MapFloatToTileIndex(r, threshold, prefabs.Count);

        return prefabs[objectIndex];
    }

    private int MapFloatToTileIndex(float value, float threshold, int tileCount)
    {
        float normalizedValue = value / threshold;

        int tileIndex = Mathf.FloorToInt(normalizedValue * tileCount);

        tileIndex = Mathf.Clamp(tileIndex, 0, tileCount - 1);

        return tileIndex;
    }
}
