using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "ScriptableObjects/WorldGenerationData", order = 1)]
public class WorldGenerationData : ScriptableObject
{
    public int worldSize;
    public int segmentCount;
    public int borderThickness;
    public BiomeType startBiome;
    public int startBiomeMinSize; // size in regions
}
