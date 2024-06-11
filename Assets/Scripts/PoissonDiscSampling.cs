using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampling
{
    public static List<Vector2Int> GeneratePoints(int radius, Vector2Int sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        int cellSize = radius / 2;

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2Int> points = new List<Vector2Int>();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2Int spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidateFloat = spawnCentre + dir * Random.Range(radius, 2 * radius);

                Vector2Int candidate = new Vector2Int(Mathf.RoundToInt(candidateFloat.x), Mathf.RoundToInt(candidateFloat.y));
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    // float perlinValue = Mathf.PerlinNoise(candidate.x * 0.03f, candidate.y * 0.03f);
                    // if (perlinValue > 0.5f)
                    // {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[candidate.x / cellSize, candidate.y / cellSize] = points.Count;
                    candidateAccepted = true;
                    break;
                    // }
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    static bool IsValid(Vector2Int candidate, Vector2Int sampleRegionSize, int cellSize, int radius, List<Vector2Int> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = candidate.x / cellSize;
            int cellY = candidate.y / cellSize;
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}