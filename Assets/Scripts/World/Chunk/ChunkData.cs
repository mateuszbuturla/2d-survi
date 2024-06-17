using UnityEngine;

public class ChunkData
{
    public Biomes biome;
    public Vector2Int biomePoint;
    public int randomSeed;

    public ChunkData(Biomes biome, Vector2Int biomePoint, int randomSeed)
    {
        this.biome = biome;
        this.biomePoint = biomePoint;
        this.randomSeed = randomSeed;
    }
}
