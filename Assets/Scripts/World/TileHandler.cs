using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileHandler : MonoBehaviour
{
    [SerializeField]
    private TileHandler Next;

    public TileBase Handle(Vector2Int pos, System.Random random)
    {
        TileBase tile = TryHandling(pos, random);

        if (tile != null)
            return tile;

        if (Next != null)
            return Next.Handle(pos, random);

        return null;
    }

    protected abstract TileBase TryHandling(Vector2Int pos, System.Random random);
}
