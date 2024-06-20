using UnityEngine;
public class FishingBobber : MonoBehaviour
{
    public float minDistance = -0.5f;
    public float maxDistance = 0.5f;
    public float speed = 2.0f;
    private bool isFishOnHook = false;
    private Vector2 startPosition;
    private Vector2 destination;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isFishOnHook)
        {
            if (destination == Vector2.zero || Vector2.Distance(transform.position, destination) < 0.1f)
            {
                float xOffset = Random.Range(minDistance, maxDistance);
                float yOffset = Random.Range(minDistance, maxDistance);

                destination = startPosition + new Vector2(xOffset, yOffset);
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, destination, step);
        }
    }

    public void FishOnHook()
    {
        isFishOnHook = true;
    }

    public void PrepareBobber(Vector2 startPosition)
    {
        this.startPosition = startPosition;
        destination = Vector2.zero;
        isFishOnHook = false;
    }

    public void ResetBobber()
    {
        destination = Vector2.zero;
        isFishOnHook = false;
    }
}