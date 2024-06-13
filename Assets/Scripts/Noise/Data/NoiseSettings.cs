using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/NoiseSettings")]
public class NoiseSettings : ScriptableObject
{
    public float noiseZoom;
    public int octaves;
    public Vector2Int offest;
    public float persistance;
    public float redistributionModifier;
    public float exponent;
}