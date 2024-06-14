using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TestData", menuName = "TestData/TestData", order = 1)]
public class TestData : ScriptableObject
{
    public TileBase[] tiles = new TileBase[9];
}
