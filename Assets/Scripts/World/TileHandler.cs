using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileHandler : MonoBehaviour
{
    [SerializeField]
    private TileHandler Next;

    public TileBase Handle(Vector2Int pos)
    {
        TileBase tile = TryHandling(pos);

        if (tile != null)
            return tile;

        if (Next != null)
            return Next.Handle(pos);

        return null;
    }

    protected abstract TileBase TryHandling(Vector2Int pos);
}
