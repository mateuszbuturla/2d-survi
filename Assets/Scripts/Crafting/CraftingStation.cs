using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    public CraftingStationType craftingStationType;
    public GameObject craftingWindow;
    public float maxDistance;
    private Player player;

    private void Start()
    {
        craftingWindow.SetActive(false);
    }

    public IEnumerator CheckIfPlayerIsOutOfRange()
    {
        bool isPlayerInRange = Utils.CheckIfInRange(player.transform.position, transform.position, maxDistance);

        if (!isPlayerInRange)
        {
            craftingWindow.SetActive(false);
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(CheckIfPlayerIsOutOfRange());
    }

    public void ShowInteractionText()
    {

    }

    public void Interact(Player player)
    {
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

        craftingWindow.SetActive(true);
    }
}
