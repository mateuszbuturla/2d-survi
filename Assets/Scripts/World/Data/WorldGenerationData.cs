using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "WorldGeneration/WorldGenerationData", order = 0)]
public class WorldGenerationData : ScriptableObject
{
    public float tileSize;
    public int chunkSize;
    public int renderDistance;
    public int worldSizeInChunk;
    [Range(0.0f, 1.0f)]
    public float terrainThreshold;
}
