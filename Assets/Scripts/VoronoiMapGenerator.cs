using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VoronoiMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] biomeTiles;
    public int size = 1000;
    public int borderThickness = 1;
    public int segmentCount = 20;

    private List<Vector3Int> points;


    void Start()
    {
        points = GenerateRandomPoints();
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++)
            {
                Vector3Int closestPoint = FindClosestPoint(new Vector3Int(x, y, 0));
                tilemap.SetTile(new Vector3Int(x, y, 0), biomeTiles[closestPoint.z]);
            }
        }
    }

    Vector3Int FindClosestPoint(Vector3Int position)
    {
        Vector3Int closestPoint = Vector3Int.zero;
        float closestDistance = float.MaxValue;

        foreach (Vector3Int point in points)
        {
            float distance = Vector3Int.Distance(position, point);
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

        int segmentSize = size / segmentCount; 

        for (int xSegment = 0; xSegment < segmentCount; xSegment++)
        {
            for (int ySegment = 0; ySegment < segmentCount; ySegment++)
            {
                int startX = xSegment * segmentSize;
                int endX = (xSegment + 1) * segmentSize;
                int startY = ySegment * segmentSize;
                int endY = (ySegment + 1) * segmentSize;

                int value = 0;
                if (xSegment <= borderThickness || xSegment >= segmentCount - borderThickness || ySegment <= borderThickness || ySegment >= segmentCount - borderThickness)
                {
                    value = 0;
                }
                else
                {
                    value = Random.Range(0, 3);
                }

                int x = Random.Range(startX, endX);
                int y = Random.Range(startY, endY);

                points.Add(new Vector3Int(x, y, value));
            }
        }

        return points;
    }
}