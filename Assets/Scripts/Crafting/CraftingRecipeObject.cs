using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeObject : MonoBehaviour
{
    public GameObject craftingIngredientPrefab;
    public GameObject ingredientsContainer;
    public Image craftingResultIcon;

    public void Prepare(CraftingRecipe craftingRecipe)
    {
        craftingResultIcon.sprite = craftingRecipe.resultItem.GetComponent<Item>().sprite;

        foreach (CraftingIngredient craftingIngredient in craftingRecipe.craftingIngredients)
        {
            GameObject newObject = Instantiate(craftingIngredientPrefab, Vector2.zero, Quaternion.identity);
            newObject.GetComponent<CraftingIngridientObject>().Prepare(craftingIngredient);
            newObject.transform.SetParent(ingredientsContainer.transform);
        }
    }
}
