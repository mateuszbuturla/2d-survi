using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "ScriptableObjects/BiomeObjectData", order = 1)]
public class BiomeObjectData : ScriptableObject
{
    public int count;
    public GameObject prefab;
}
