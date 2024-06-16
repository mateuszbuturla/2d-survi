using UnityEngine;
using UnityEngine.Tilemaps;

public class SubBiomeHandler : MonoBehaviour
{
    [SerializeField]
    private SubBiomeHandler Next;
    public TileHandler startTileHandler;
    public TileDecorationHandler startDecorationHandler;
    public ObjectHandler startObjectHandler;

    public (TileBase, TileBase, GameObject) Handle(Vector2Int pos, ref System.Random random)
    {
        (TileBase, TileBase, GameObject) result = TryHandling(pos, ref random);

        if (result.Item1 != null)
        {
            return result;
        }

        if (Next != null)
        {
            return Next.Handle(pos, ref random);
        }

        return (null, null, null);
    }

    protected (TileBase, TileBase, GameObject) TryHandling(Vector2Int pos, ref System.Random random)
    {
        TileBase tile = GetBaseTile(pos, ref random);
        TileBase decorationTile = null;
        GameObject worldObject = null;

        if (startDecorationHandler)
        {
            decorationTile = startDecorationHandler.Handle(pos, ref random);
        }

        if (startObjectHandler)
        {
            worldObject = startObjectHandler.Handle(pos, ref random);
        }

        return (tile, decorationTile, worldObject);
    }

    private TileBase GetBaseTile(Vector2Int pos, ref System.Random random)
    {
        TileBase result = null;

        if (startTileHandler)
        {
            result = startTileHandler.Handle(pos, ref random);
        }

        return result;
    }
}
