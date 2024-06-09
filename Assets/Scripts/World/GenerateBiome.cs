using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateBiome : MonoBehaviour
{
    public TileBase baseTile;

    public void GenerateTile(Vector3Int pos)
    {
        VoronoiMapGenerator generateWorld = transform.parent.GetComponent<VoronoiMapGenerator>();
        generateWorld.tilemap.SetTile(pos, baseTile);
    }
}
