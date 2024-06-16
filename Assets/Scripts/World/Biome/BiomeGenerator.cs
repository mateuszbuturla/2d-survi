using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeGenerator : MonoBehaviour
{
    public BiomeType biomeType;
    public Biomes biome;
    public TileBase baseTile;
    public TileHandler startTileHandler;

    public TileBase GetTile(Vector2Int pos, ref System.Random random)
    {
        TileBase result = null;

        if (startTileHandler)
        {
            result = startTileHandler.Handle(pos, ref random);
        }

        if (result == null)
        {
            return baseTile;
        }

        return result;
    }
}
