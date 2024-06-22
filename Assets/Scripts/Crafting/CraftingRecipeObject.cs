using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeObject : MonoBehaviour
{
    private CraftingController craftingController;
    public Image craftingResultIcon;
    public TextMeshProUGUI craftingResultName;
    private CraftingRecipe craftingRecipe;

    public void HandleClick()
    {
        craftingController.SelectRecipe(craftingRecipe);
    }

    public void Prepare(CraftingRecipe craftingRecipe, CraftingController craftingController)
    {
        this.craftingController = craftingController;
        this.craftingRecipe = craftingRecipe;
        craftingResultIcon.sprite = craftingRecipe.resultItem.GetComponent<Item>().sprite;
        craftingResultName.text = craftingRecipe.resultItem.GetComponent<Item>().name;
    }

}
