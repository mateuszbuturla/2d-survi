using UnityEngine;

public static class Utils
{
    public static Vector2 GetMousePoision()
    {
        Vector2 screenPosition = Input.mousePosition;

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        return new Vector2(worldPosition.x, worldPosition.y);
    }

    public static Vector2Int GetMousePoisionInt()
    {
        Vector2 mousePosition = GetMousePoision();

        return new Vector2Int((int)mousePosition.x, (int)mousePosition.y);
    }
}
