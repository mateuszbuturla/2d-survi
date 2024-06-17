using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeGenerator : MonoBehaviour
{
    public BiomeType biomeType;
    public Biomes biome;
    public TileBase baseTile;
    public SubBiomeHandler startSubBiomeHandler;
    public TileDecorationHandler startDecorationHandler;
    public ObjectHandler startObjectHandler;

    public (TileBase, TileBase, GameObject) GetTile(Vector2Int pos, ref System.Random random)
    {
        if (startSubBiomeHandler != null)
        {
            var result = startSubBiomeHandler.Handle(pos, ref random);

            if (result.Item1 != null)
            {
                return result;
            }
        }

        TileBase tile = baseTile;
        TileBase decorationTile = GetDecorationTile(pos, ref random);
        GameObject worldObject = GetObject(pos, ref random);

        return (tile, decorationTile, worldObject);
    }

    private TileBase GetBaseTile(Vector2Int pos, ref System.Random random)
    {
        TileBase result = null;

        if (startSubBiomeHandler)
        {
            // result = startTileHandler.Handle(pos, ref random);
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

    private GameObject GetObject(Vector2Int pos, ref System.Random random)
    {
        GameObject result = null;

        if (startObjectHandler)
        {
            result = startObjectHandler.Handle(pos, ref random);
        }

        return result;
    }
}
