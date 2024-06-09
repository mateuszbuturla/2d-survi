using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoronoiEdgeDistortion
{
    public static Vector3Int Get2DTurbulence(Vector2 input, VoronoiDistortionData voronoiDistortionData)
    {
        input = input / voronoiDistortionData.baseScale + voronoiDistortionData.seed;
        float a = 2f * voronoiDistortionData.amplitude;

        Vector3Int noise = Vector3Int.zero;

        for (int octave = 0; octave < voronoiDistortionData.octaveCount; octave++)
        {
            noise.x += (int)(a * (Mathf.PerlinNoise(input.x, input.y) - 0.5f));
            noise.y += (int)(a * (Mathf.PerlinNoise(input.x + voronoiDistortionData.seed.y, input.y + voronoiDistortionData.seed.y) - 0.5f));
            input = input * voronoiDistortionData.lacunarity + voronoiDistortionData.seed;
            a *= voronoiDistortionData.persistence;
        }

        return noise;
    }
}
