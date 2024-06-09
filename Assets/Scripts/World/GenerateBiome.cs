using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateBiome : MonoBehaviour
{
    public TileBase baseTile;

    public void GenerateTile(Vector3Int pos)
    {
        GenerateWorld generateWorld = transform.parent.GetComponent<GenerateWorld>();
        generateWorld.tilemap.SetTile(pos, baseTile);
    }
}
