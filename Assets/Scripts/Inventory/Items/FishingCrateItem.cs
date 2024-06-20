using UnityEngine;

public class FishingCrateItem : Openable
{
    public Item[] itemPool;
    public int itemRolls = 1;

    public override void RightClickEffect()
    {
        Inventory targetInventory = GetContainingInventory();

        for (int i = 0; i < itemRolls; i++)
        {
            int random = Random.Range(0, itemPool.Length);
            targetInventory.AddItem(Instantiate(itemPool[random]));
        }


        int slotId = GetContainingItemSlot().id;
        targetInventory.DecreateItemCount(slotId);
    }
}
