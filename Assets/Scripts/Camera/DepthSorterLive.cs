using UnityEngine;

public class DepthSorterLive : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    void Update()
    {
        spriteRenderer.sortingOrder = (int)(-transform.position.y);
    }
}
