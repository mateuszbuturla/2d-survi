using UnityEngine;
using static Item;

[CreateAssetMenu(fileName = "New ItemSO", menuName = "ScriptableObjects/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{
    public int id;
    public new string name;
    public Type type;
    public Rarity rarity;
    public Sprite sprite;

    public int maxStack;

    public bool hasDurability;
    public int maxDurability;
    public int startingDurability;
}