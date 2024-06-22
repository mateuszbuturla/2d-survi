using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWindow : MonoBehaviour
{
    public GameObject craftingRecipeContainer;
    public GameObject craftingRecipePrefab;
    public Image recipeItemImage;
    public GameObject ingredientContainer;
    public GameObject craftingIngredientPrefab;

    private List<GameObject> recipeObjects = new List<GameObject>();
    private List<GameObject> ingredientObjects = new List<GameObject>();

    public void AddCraftingRecipe(CraftingRecipe craftingRecipe, CraftingController craftingController)
    {
        GameObject newRecipe = Instantiate(craftingRecipePrefab, Vector2.zero, Quaternion.identity);
        newRecipe.GetComponent<CraftingRecipeObject>().Prepare(craftingRecipe, craftingController);
        newRecipe.transform.SetParent(craftingRecipeContainer.transform);

        recipeObjects.Add(newRecipe);
    }

    public void RemoveAllCraftingRecipes()
    {
        foreach (GameObject recipe in recipeObjects)
        {
            Destroy(recipe);
        }

        recipeObjects = new List<GameObject>();
    }

    public void DisplayRecipeDetails(CraftingRecipe craftingRecipe, CraftingController craftingController)
    {
        foreach (GameObject ingredient in ingredientObjects)
        {
            Destroy(ingredient);
        }
        ingredientObjects = new List<GameObject>();

        recipeItemImage.sprite = craftingRecipe.resultItem.GetComponent<Item>().sprite;

        foreach (CraftingIngredient craftingIngredient in craftingRecipe.craftingIngredients)
        {
            GameObject newIngredient = Instantiate(craftingIngredientPrefab, Vector2.zero, Quaternion.identity);
            newIngredient.transform.SetParent(ingredientContainer.transform);
            newIngredient.GetComponent<CraftingIngridientObject>().Prepare(craftingIngredient, craftingController);
            ingredientObjects.Add(newIngredient);
        }
    }
}
