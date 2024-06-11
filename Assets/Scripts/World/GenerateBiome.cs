using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateBiome : MonoBehaviour
{
    public TileBase baseTile;

    [SerializeField]
    public List<BiomeObjectData> objects = new List<BiomeObjectData>();

    void Start()
    {

        // if (!tree)
        // {
        //     return;
        // }

        // points = PoissonDiscSampling.GeneratePoints(4, new Vector2Int(250, 250));

        // foreach (Vector2Int obj in points)
        // {
        //     VoronoiMapGenerator generateWorld = transform.parent.GetComponent<VoronoiMapGenerator>();
        //     Vector3 worldPosition = generateWorld.tilemap.CellToWorld(new Vector3Int(obj.x, obj.y, 0));
        //     Instantiate(tree, worldPosition + new Vector3(0, 0, 0), Quaternion.Euler(45, 0, 0));
        // }
    }

    public void GenerateTile(Vector3Int pos)
    {
        VoronoiMapGenerator generateWorld = transform.parent.GetComponent<VoronoiMapGenerator>();
        generateWorld.tilemap.SetTile(pos, baseTile);

        //GenerateObject(pos);
    }

    // private void GenerateObject(Vector3Int pos)
    // {
    //     foreach (var obj in objects)
    //     {
    //         int rn = Random.Range(0, 10000);

    //         if (rn <= obj.chance)
    //         {
    //             VoronoiMapGenerator generateWorld = transform.parent.GetComponent<VoronoiMapGenerator>();

    //             Vector3 worldPosition = generateWorld.tilemap.CellToWorld(pos);
    //             Instantiate(obj.prefab, worldPosition + new Vector3(0, 0, 0), Quaternion.Euler(45, 0, 0));
    //             break;
    //         }
    //     }
    // }
}
