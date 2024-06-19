﻿using UnityEngine;

public interface IAcceptItem
{
    // -- Return replaced item, if an item is being replaced.
    public abstract Item AcceptItem(Item item, ItemSlot sourceItemSlot);
}