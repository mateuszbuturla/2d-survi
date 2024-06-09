using System.Collections;
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
}