using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngridientObject : MonoBehaviour
{
    public TextMeshProUGUI amountLabel;
    public Image image;

    public void Prepare(CraftingIngredient craftingIngredient)
    {
        image.sprite = craftingIngredient.item.GetComponent<Item>().sprite;
        amountLabel.text = craftingIngredient.count.ToString();
    }
}
