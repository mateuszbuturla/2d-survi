using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileDecorationHandler : MonoBehaviour
{
    [SerializeField]
    private TileHandler Next;

    public TileBase Handle(Vector2Int pos, System.Random random, TileBase tile)
    {
        TileBase t = TryHandling(pos, random, tile);

        if (t != null)
            return t;

        if (Next != null)
            return Next.Handle(pos, random);

        return null;
    }

    protected abstract TileBase TryHandling(Vector2Int pos, System.Random random, TileBase tile);
}
