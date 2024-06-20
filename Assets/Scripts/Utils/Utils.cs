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

    public static bool CheckIfInRange(Vector2 pos1, Vector2 pos2, float distance)
    {
        return Vector2.Distance(pos1, pos2) <= distance;
    }
}
