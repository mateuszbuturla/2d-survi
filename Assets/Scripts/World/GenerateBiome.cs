using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateBiome : MonoBehaviour
{
    public TileBase baseTile;

    [SerializeField]
    public List<BiomeObjectData> objects = new List<BiomeObjectData>();

    public void GenerateTile(Vector3Int pos)
    {
        VoronoiMapGenerator generateWorld = transform.parent.GetComponent<VoronoiMapGenerator>();
        generateWorld.tilemap.SetTile(pos, baseTile);

        GenerateObject(pos);

    }

    private void GenerateObject(Vector3Int pos)
    {
        foreach (var obj in objects)
        {
            int rn = Random.Range(0, 1000);

            if (rn <= obj.chance)
            {
                VoronoiMapGenerator generateWorld = transform.parent.GetComponent<VoronoiMapGenerator>();

                Vector3 worldPosition = generateWorld.tilemap.CellToWorld(pos);
                Instantiate(obj.prefab, worldPosition, Quaternion.identity);
                break;
            }
        }
    }
}
