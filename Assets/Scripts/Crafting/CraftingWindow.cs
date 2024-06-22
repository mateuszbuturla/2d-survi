using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public GameObject craftingRecipeContainer;
    public GameObject craftingRecipePrefab;

    private List<GameObject> recipeObjects = new List<GameObject>();

    public void AddCraftingRecipe(CraftingRecipe craftingRecipe, CraftingController craftingController)
    {
        GameObject newRecipe = Instantiate(craftingRecipePrefab);
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
}
