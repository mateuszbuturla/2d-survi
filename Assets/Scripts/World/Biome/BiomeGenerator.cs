using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeGenerator : MonoBehaviour
{
    public BiomeType biomeType;
    public Biomes biome;
    public TileBase baseTile;
    public TileHandler startTileHandler;
    public TileDecorationHandler startDecorationHandler;

    public (TileBase, TileBase) GetTile(Vector2Int pos, ref System.Random random)
    {
        TileBase tile = GetBaseTile(pos, ref random);
        TileBase decorationTile = null;

        if (tile == baseTile)
        {
            decorationTile = GetDecorationTile(pos, ref random);
        }

        return (tile, decorationTile);
    }

    private TileBase GetBaseTile(Vector2Int pos, ref System.Random random)
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

    private TileBase GetDecorationTile(Vector2Int pos, ref System.Random random)
    {
        TileBase result = null;

        if (startDecorationHandler)
        {
            result = startDecorationHandler.Handle(pos, ref random);
        }

        return result;
    }
}
