using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngridientObject : MonoBehaviour
{
    public TextMeshProUGUI amountLabel;
    public Image image;

    public void Prepare(CraftingIngredient craftingIngredient, CraftingController craftingController)
    {
        image.sprite = craftingIngredient.item.GetComponent<Item>().sprite;

        int ownedAmount = craftingController.GetItemCount(craftingIngredient.item.GetComponent<Item>());
        int requiredAmount = craftingIngredient.count;

        amountLabel.text = $"{ownedAmount}/{requiredAmount}";
        amountLabel.color = ownedAmount >= requiredAmount ? Color.green : Color.red;
    }
}
