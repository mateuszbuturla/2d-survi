using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D fishingCursor;
    public Tilemap tilemap;
    public TileBase waterTile;

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        Vector2Int mousePos = Utils.GetMousePoisionInt();

        TileBase tile = tilemap.GetTile((Vector3Int)mousePos);

        if (tile == waterTile)
        {
            SetFishingCursor();
        }
        else
        {
            OnMouseExit();
        }
    }

    private void SetFishingCursor()
    {
        Cursor.SetCursor(fishingCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
