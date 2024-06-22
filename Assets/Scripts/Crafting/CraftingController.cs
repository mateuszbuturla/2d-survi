using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    private bool isOpen;
    public Dictionary<int, GameObject> workingStationsInRange = new Dictionary<int, GameObject>();
    private GameObject craftingWindow;

    private void Start()
    {
        craftingWindow = Singleton.instance.craftingWindow;
        craftingWindow.SetActive(isOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isOpen = !isOpen;
            craftingWindow.SetActive(isOpen);

            if (isOpen) RenderRecipes();
            else ClearRecipes();
        }
    }

    private void RenderRecipes()
    {
        Inventory playerInventory = GetComponent<Player>().playerInventory;

        List<CraftingStationType> avaiableCraftingStations = new List<CraftingStationType>()
        {
            CraftingStationType.HAND,
        };

        foreach (var vkp in workingStationsInRange)
        {
            CraftingStation craftingStation = vkp.Value.GetComponent<CraftingStation>();
            CraftingStationType craftingStationType = craftingStation.craftingStationType;

            if (!avaiableCraftingStations.Contains(craftingStationType))
            {
                avaiableCraftingStations.Add(craftingStationType);
            }
        }

        List<CraftingRecipe> recipes = Singleton.instance.allRecipes.recipes;
        List<CraftingRecipe> avaiableToCraftRecipes = recipes.Where(r =>
        {
            if (!avaiableCraftingStations.Contains(r.requiredCraftingStation))
            {
                return false;
            }

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
    }

    private void RefreshCraftginWindow()
    {
        ClearRecipes();
        RenderRecipes();
    }

    private void ClearRecipes()
    {
        craftingWindow.GetComponent<CraftingWindow>().RemoveAllCraftingRecipes();
    }

    public void HandleCraftItem(CraftingRecipe craftingRecipe)
    {
        Inventory playerInventory = GetComponent<Player>().playerInventory;

        foreach (CraftingIngredient craftingIngredient in craftingRecipe.craftingIngredients)
        {
            playerInventory.DecreaseItemCount(craftingIngredient.item.GetComponent<Item>(), craftingIngredient.count);
        }

        playerInventory.AddItem(craftingRecipe.resultItem);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CraftingStation" && other.TryGetComponent<CraftingStation>(out CraftingStation craftingStation))
        {
            workingStationsInRange[other.GetInstanceID()] = other.gameObject;
            RefreshCraftginWindow();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "CraftingStation" && other.TryGetComponent<CraftingStation>(out CraftingStation craftingStation))
        {
            int ojectId = other.GetInstanceID();

            if (workingStationsInRange.ContainsKey(ojectId))
            {
                workingStationsInRange.Remove(ojectId);
                RefreshCraftginWindow();
            }
        }
    }
}
