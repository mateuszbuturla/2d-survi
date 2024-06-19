using UnityEngine;

public class FishingString : MonoBehaviour
{
    public Transform point1;
    public Transform point2;

    private LineRenderer lineRenderer;
    public int numPoints = 50;
    public float curveHeight = 1.0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void UpdateLinePositions()
    {
        Vector3 startPoint = point1.position;
        Vector3 endPoint = point2.position;

        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1f);
            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);

            point.y -= Mathf.Sin(t * Mathf.PI) * curveHeight;

            lineRenderer.SetPosition(i, point);
        }
    }

    void Update()
    {
        UpdateLinePositions();
    }
}
