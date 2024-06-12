public static class WorldGenerationHelper
{
    public static int GetWorldSegmentSize(WorldGenerationData wgd)
    {
        return wgd.worldSize / wgd.segmentCount;
    }

    public static int GetWorldCenter(WorldGenerationData wgd)
    {
        return (int)(wgd.segmentCount / 2);
    }
}
