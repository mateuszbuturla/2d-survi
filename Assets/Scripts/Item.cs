using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public int id;
    public new string name;
    public Sprite sprite;

    public int maxStack;
    public int amount;
}
