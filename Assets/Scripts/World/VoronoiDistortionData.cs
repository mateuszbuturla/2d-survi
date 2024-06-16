using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "ScriptableObjects/VoronoiDistortionData", order = 1)]
public class VoronoiDistortionData : ScriptableObject
{
    public float baseScale;
    public int octaveCount;
    public float amplitude;
    public float lacunarity;
    public float persistence;
    public Vector2 seed;
}
