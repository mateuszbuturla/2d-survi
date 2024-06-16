using UnityEngine;

public class TreeHandler : ObjectHandler
{
    [Range(0f, 100f)]
    public float threshold;

    public GameObject prefab;

    protected override GameObject TryHandling(Vector2Int pos, ref System.Random random)
    {
        float r = (float)(random.NextDouble() * 100);

        if (r >= threshold)
        {
            return null;
        }

        return prefab;
    }
}
