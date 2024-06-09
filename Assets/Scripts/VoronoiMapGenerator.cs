using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VoronoiMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] biomeTiles;
    public int size;
    public int borderThickness;
    public int segmentCount;

    private List<Vector3Int> points;


    public float baseScale = 50.0f;
    public int octaveCount = 4;
    public float amplitude = 5.0f;
    public float lacunarity = 2.0f;
    public float persistence = 0.5f;
    public Vector2 seed = new Vector2(-71, 37);

    void Start()
    {
        points = GenerateRandomPoints();
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3Int closestPoint = FindClosestPoint(new Vector3Int(x, y, 0));
                tilemap.SetTile(new Vector3Int(x, y, 0), biomeTiles[closestPoint.z]);
            }
        }
    }

    Vector3Int Get2DTurbulence(Vector2 input)
    {

        input = input / baseScale + seed;
        float a = 2f * amplitude;

        Vector3Int noise = Vector3Int.zero;

        for (int octave = 0; octave < octaveCount; octave++)
        {
            noise.x += (int)(a * (Mathf.PerlinNoise(input.x, input.y) - 0.5f));
            noise.y += (int)(a * (Mathf.PerlinNoise(input.x + seed.y, input.y + seed.y) - 0.5f));
            input = input * lacunarity + seed;
            a *= persistence;
        }

        return noise;
}

    Vector3Int FindClosestPoint(Vector3Int position)
    {
        Vector3Int closestPoint = Vector3Int.zero;
        float closestDistance = float.MaxValue;

        Vector3Int newPos = position + Get2DTurbulence(new Vector2Int(position.x, position.y));

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