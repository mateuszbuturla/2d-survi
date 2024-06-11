using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Biome
{
    public BiomeType biomeType;
    public Vector2Int mainBiomePoint;
    public List<Vector2Int> biomePoints = new List<Vector2Int>();

    public Biome(BiomeType biomeType, Vector2Int biomePoint)
    {
        this.biomeType = biomeType;
        this.mainBiomePoint = biomePoint;
    }
}
