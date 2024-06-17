using UnityEngine;

public abstract class ObjectHandler : MonoBehaviour
{
    [SerializeField]
    private ObjectHandler Next;

    public GameObject Handle(Vector2Int pos, ref System.Random random)
    {
        GameObject t = TryHandling(pos, ref random);

        if (t != null)
            return t;

        if (Next != null)
            return Next.Handle(pos, ref random);

        return null;
    }

    protected abstract GameObject TryHandling(Vector2Int pos, ref System.Random random);
}
