using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BiomeData
{
    public BiomeTier biomeTier;
    public BiomeType biomeType;
    public int count;
    public GenerateBiome biomeGenerator;
}