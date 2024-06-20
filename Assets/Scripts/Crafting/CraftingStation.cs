using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    public CraftingStationType craftingStationType;
    public GameObject craftingWindow;
    public float maxDistance;
    private Player player;
    private bool isActive;

    private void Start()
    {
        craftingWindow.SetActive(false);
    }

    public IEnumerator CheckIfPlayerIsOutOfRange()
    {
        bool isPlayerInRange = Utils.CheckIfInRange(player.transform.position, transform.position, maxDistance);

        if (!isPlayerInRange)
        {
            craftingWindow.GetComponent<CraftingWindow>().RemoveAllCraftingRecipes();
            craftingWindow.SetActive(false);
            isActive = false;
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(CheckIfPlayerIsOutOfRange());
    }

    public void HandleCraftItem(CraftingRecipe craftingRecipe)
    {
        player.playerInventory.AddItem(craftingRecipe.resultItem);
    }

    public void ShowInteractionText()
    {

    }

    public void Interact(Player player)
    {
        if (isActive)
        {
            return;
        }

        isActive = true;
        this.player = player;
        StartCoroutine(CheckIfPlayerIsOutOfRange());

        Inventory playerInventory = player.playerInventory;

        List<CraftingRecipe> recipes = Singleton.instance.allRecipes.recipes;
        List<CraftingRecipe> filterRecipes = recipes.Where(r => r.requiredCraftingStation == craftingStationType).ToList();
        List<CraftingRecipe> avaiableToCraftRecipes = filterRecipes.Where(r =>
        {
            foreach (CraftingIngredient craftingIngredient in r.craftingIngredients)
            {
                bool contains = playerInventory.CheckIfContainsItem(craftingIngredient.item.GetComponent<Item>(), craftingIngredient.count);

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }).ToList();

        foreach (CraftingRecipe craftingRecipe in avaiableToCraftRecipes)
        {
            craftingWindow.GetComponent<CraftingWindow>().AddCraftingRecipe(craftingRecipe, this);
        }

        craftingWindow.SetActive(true);
    }
}
