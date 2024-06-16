using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour 
{
    public static Singleton instance;
    void Awake() { if (instance == null) { instance = this; } }

    public List<GameObject> players;
    public GameObject droppedItemPrefab;
}