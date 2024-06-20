using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeObject : MonoBehaviour
{
    public CraftingStation craftingStation;
    public GameObject craftingIngredientPrefab;
    public GameObject ingredientsContainer;
    public Image craftingResultIcon;
    private CraftingRecipe craftingRecipe;
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    public void HandleClick()
    {
        clicked++;

        if (clicked == 1)
            clicktime = Time.time;

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            craftingStation.HandleCraftItem(craftingRecipe);
        }
        else if (clicked > 2 || Time.time - clicktime > 1)
            clicked = 0;
    }

    public void Prepare(CraftingRecipe craftingRecipe, CraftingStation craftingStation)
    {
        this.craftingStation = craftingStation;
        this.craftingRecipe = craftingRecipe;
        craftingResultIcon.sprite = craftingRecipe.resultItem.GetComponent<Item>().sprite;

        foreach (CraftingIngredient craftingIngredient in craftingRecipe.craftingIngredients)
        {
            GameObject newObject = Instantiate(craftingIngredientPrefab, Vector2.zero, Quaternion.identity);
            newObject.GetComponent<CraftingIngridientObject>().Prepare(craftingIngredient);
            newObject.transform.SetParent(ingredientsContainer.transform);
        }
    }

}
