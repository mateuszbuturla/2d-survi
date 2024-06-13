using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileHandler : MonoBehaviour
{
    [SerializeField]
    private TileHandler Next;
    public TileBase tile;

    public TileBase Handle(Vector2Int pos)
    {
        if (TryHandling(pos))
            return tile;

        if (Next != null)
            return Next.Handle(pos);

        return null;
    }

    protected abstract bool TryHandling(Vector2Int pos);
}
