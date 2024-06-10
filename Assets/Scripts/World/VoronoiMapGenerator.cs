﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VoronoiMapGenerator : MonoBehaviour
{
    public WorldGenerationData worldGenerationData;
    public VoronoiDistortionData voronoiDistortionData;

    public Tilemap tilemap;

    [SerializeField]
    public List<BiomeData> biomeGeneratorsData = new List<BiomeData>();

    private List<Vector3Int> points;
    private List<Vector3Int> connected = new List<Vector3Int>();

    public GenerateBiome roadGenerator;

    void Start()
    {
        points = GenerateRandomPoints();
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < worldGenerationData.worldWidth; x++)
        {
            for (int y = 0; y < worldGenerationData.worldHeight; y++)
            {
                Vector3Int closestPoint = FindClosestPoint(new Vector3Int(x, y, 0));
                biomeGeneratorsData[closestPoint.z].biomeGenerator.GenerateTile(new Vector3Int(x, y, 0));
            }
        }

        foreach (Vector3Int point in points)
        {
            if (point.z != 0)
            {
                Vector3Int fcp = FCP(point);

                if (fcp.z != 0)
                {
                    connected.Add(point);
                    List<Vector2Int> road = GetLine(new Vector2Int(point.x, point.y), new Vector2Int(fcp.x, fcp.y));

                    foreach (Vector2Int rPoint in road)
                    {
                        roadGenerator.GenerateTile(new Vector3Int(rPoint.x, rPoint.y, 0));
                    }
                }


                // List<Vector3Int> closePoints = FindClosePoints(point, points, 10, 25);

                // int c = 0;

                // foreach (Vector3Int cPoint in closePoints)
                // {
                //     if (c < 1)
                //     {
                // List<Vector2Int> road = GetLine(new Vector2Int(point.x, point.y), new Vector2Int(cPoint.x, cPoint.y));

                // foreach (Vector2Int rPoint in road)
                // {
                //     roadGenerator.GenerateTile(new Vector3Int(rPoint.x, rPoint.y, 0));
                // }
                //     }
                //     c++;
                // }
            }
        }
    }

    Vector3Int FindClosestPoint(Vector3Int position)
    {
        Vector3Int closestPoint = Vector3Int.zero;
        float closestDistance = float.MaxValue;

        Vector3Int newPos = position + VoronoiEdgeDistortion.Get2DTurbulence(new Vector2Int(position.x, position.y), voronoiDistortionData);

        foreach (Vector3Int point in points)
        {
            float distance = Vector3Int.Distance(newPos, point);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    List<Vector3Int> GenerateRandomPoints()
    {
        List<Vector3Int> points = new List<Vector3Int>();

        int segmentSizeX = worldGenerationData.worldWidth / worldGenerationData.segmentCount;
        int segmentSizeY = worldGenerationData.worldHeight / worldGenerationData.segmentCount;

        for (int xSegment = 0; xSegment < worldGenerationData.segmentCount; xSegment++)
        {
            for (int ySegment = 0; ySegment < worldGenerationData.segmentCount; ySegment++)
            {
                int startX = xSegment * segmentSizeX;
                int endX = (xSegment + 1) * segmentSizeX;
                int startY = ySegment * segmentSizeY;
                int endY = (ySegment + 1) * segmentSizeY;

                int value = 0;
                if (xSegment <= worldGenerationData.borderThickness || xSegment >= worldGenerationData.segmentCount - worldGenerationData.borderThickness || ySegment <= worldGenerationData.borderThickness || ySegment >= worldGenerationData.segmentCount - worldGenerationData.borderThickness)
                {
                    value = 0;
                }
                else
                {
                    value = Random.Range(0, biomeGeneratorsData.Count);
                }

                int x = Random.Range(startX, endX);
                int y = Random.Range(startY, endY);

                points.Add(new Vector3Int(x, y, value));
            }
        }

        return points;
    }

    private List<Vector3Int> FindClosePoints(Vector3Int targetPoint, List<Vector3Int> points, float minDistance, float maxDistance)
    {
        List<Vector3Int> closePoints = new List<Vector3Int>();

        foreach (var point in points)
        {
            if (point != targetPoint && point.z != 0)
            {
                float distance = Vector2.Distance(new Vector2(targetPoint.x, targetPoint.y), new Vector2(point.x, point.y));

                if (distance < maxDistance && distance > minDistance)
                {
                    closePoints.Add(point);
                }
            }
        }

        return closePoints;
    }

    public List<Vector2Int> GetLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);

        int sx = start.x < end.x ? 1 : -1;
        int sy = start.y < end.y ? 1 : -1;

        int err = dx - dy;

        int x = start.x;
        int y = start.y;

        while (true)
        {
            points.Add(new Vector2Int(x, y));

            if (x == end.x && y == end.y)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }

        return points;
    }

    Vector3Int FCP(Vector3Int position)
    {
        Vector3Int closestPoint = Vector3Int.zero;
        float closestDistance = float.MaxValue;

        foreach (Vector3Int point in points)
        {
            float distance = Vector3Int.Distance(position, point);
            if (distance < closestDistance && point != position && !connected.Contains(point) && point.z != 0)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

}