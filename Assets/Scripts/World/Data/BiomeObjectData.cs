using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "ScriptableObjects/BiomeObjectData", order = 1)]
public class BiomeObjectData : ScriptableObject
{
    public int radius;
    public int numSamplesBeforeRejection;
    public GameObject prefab;
}
