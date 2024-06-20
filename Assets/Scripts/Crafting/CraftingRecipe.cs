using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting/CraftingRecipe", order = 0)]
public class CraftingRecipe : ScriptableObject
{
    public GameObject resultItem;
    public CraftingStationType requiredCraftingStation;
    public List<CraftingIngredient> craftingIngredients;
}
