using UnityEngine;

public class ChunkData
{
    public Biomes biome;
    public Vector2Int biomePoint;

    public ChunkData(Biomes biome, Vector2Int biomePoint)
    {
        this.biome = biome;
        this.biomePoint = biomePoint;
    }
}
