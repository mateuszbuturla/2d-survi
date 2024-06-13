using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "ScriptableObjects/BiomeObjectData", order = 1)]
public class BiomeObjectData : ScriptableObject
{
    public bool staticCount;
    public int count;
    public int numSamplesBeforeRejection;
    public int minDistanceBetween;
    public GameObject prefab;
}
