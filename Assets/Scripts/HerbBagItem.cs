using UnityEngine;

public class HerbBagItem : Openable
{
    public Item[] itemPool;
    public int itemRolls = 1;   //amount of times to roll for item

    public override void RightClickEffect()
    {
        Inventory targetInventory = GetContainingInventory();

        for (int i = 0; i < itemRolls; i++)
        {
            // can replace index '0' with a weighted random, or use a weighted random for item rolls with multilpe rarity pools
            targetInventory.AddItem(Instantiate(itemPool[0]));
        }

        // -- not on my branch yet, but uncomment on yours
        // targetInventory.RemoveItem(this);
    }
}