using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileDecorationHandler : MonoBehaviour
{
    [SerializeField]
    private TileDecorationHandler Next;

    public TileBase Handle(Vector2Int pos, ref System.Random random)
    {
        TileBase t = TryHandling(pos, ref random);

        if (t != null)
            return t;

        if (Next != null)
            return Next.Handle(pos, ref random);

        return null;
    }

    protected abstract TileBase TryHandling(Vector2Int pos, ref System.Random random);
}
