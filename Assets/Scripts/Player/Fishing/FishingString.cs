using UnityEngine;

public class FishingString : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public int numPoints = 50;
    public float maxCurveHeight = 1.0f;
    public float transitionSpeed = 2.0f;
    public bool isBobberInWater = false;

    private LineRenderer lineRenderer;
    private float currentCurveHeight = 0.0f;
    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();


        lineRenderer.positionCount = numPoints;


        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        UpdateLinePositions();
    }

    void Update()
    {
        if (isBobberInWater)
        {
            currentCurveHeight = Mathf.Lerp(currentCurveHeight, maxCurveHeight, Time.deltaTime * transitionSpeed);
        }
        else
        {
            currentCurveHeight = 0.0f;
        }

        UpdateLinePositions();

    }

    void UpdateLinePositions()
    {
        Vector3 startPoint = point1.position;
        Vector3 endPoint = point2.position;

        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1f);
            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);

            point.y -= Mathf.Sin(t * Mathf.PI) * currentCurveHeight;

            lineRenderer.SetPosition(i, point);
        }
    }
}
