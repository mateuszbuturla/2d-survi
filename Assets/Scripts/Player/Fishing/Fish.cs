using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "Fishing/Fish", order = 0)]
public class Fish : ScriptableObject
{
    public string fishName;
    public Sprite sprite;
    public FishRarity fishRarity;
    public GameObject item;
}
