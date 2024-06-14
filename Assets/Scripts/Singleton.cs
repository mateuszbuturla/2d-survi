using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour 
{
    public static Singleton instance;

    public List<GameObject> players;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}