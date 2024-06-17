using System.Collections.Generic;
using UnityEngine;

public enum ObjectNoiseHandlerType
{
    LOCAL_MAXIMA,
    LOCAL_MINIMA
}

public class ObjectNoiseHandler : ObjectHandler
{
    public ObjectNoiseHandlerType type;
    public NoiseSettings noiseSettings;

    [Range(0f, 1.0f)]
    public float threshold;

    public List<GameObject> prefabs;

    List<Vector2Int> dircetionsToCheck = new List<Vector2Int>() {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
    };

    protected override GameObject TryHandling(Vector2Int pos, ref System.Random random)
    {
        if (!IsLocalExtrema(pos))
        {
            return null;
        }

        return prefabs[0];
    }

    bool IsLocalExtrema(Vector2Int pos)
    {
        float currentValue = GeneratePerlinNoiseValue(pos);
        float[] neighborValues = new float[dircetionsToCheck.Count];

        for (int i = 0; i < dircetionsToCheck.Count; i++)
        {
            neighborValues[i] = GeneratePerlinNoiseValue(pos + dircetionsToCheck[i]);
        }

        bool isExtrema = type == ObjectNoiseHandlerType.LOCAL_MAXIMA ?
                         IsLocalMaxima(currentValue, neighborValues) :
                         IsLocalMinima(currentValue, neighborValues);

        return isExtrema;
    }

    bool IsLocalMaxima(float currentValue, float[] neighborValues)
    {
        foreach (float neighborValue in neighborValues)
        {
            if (currentValue <= neighborValue)
            {
                return false;
            }
        }

        return currentValue > threshold;
    }

    bool IsLocalMinima(float currentValue, float[] neighborValues)
    {
        foreach (float neighborValue in neighborValues)
        {
            if (currentValue >= neighborValue)
            {
                return false;
            }
        }

        return currentValue < threshold;
    }

    float GeneratePerlinNoiseValue(Vector2Int pos)
    {
        return MyNoise.OctavePerlin(pos.x, pos.y, noiseSettings);
    }
}