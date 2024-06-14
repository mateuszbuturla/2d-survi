using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public List<GameObject> itemSlots;
    public GameObject itemSlotPrefab;

    public int slots;
    public int rows;
    public int columns;

    void Start()
    {
        itemSlots = new();
        for (int i = 0; i < slots; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab);
            itemSlot.transform.SetParent(transform);
            itemSlot.GetComponent<ItemSlot>().id = i;

            itemSlots.Add(itemSlot);
        }

        // -- DEBUG --
        itemSlots[0].GetComponent<ItemSlot>().AcceptItem(GameObject.Find("Bow").GetComponent<Bow>(), null);
        itemSlots[1].GetComponent<ItemSlot>().AcceptItem(GameObject.Find("Sword").GetComponent<Sword>(), null);
        // -- DEBUG --

        AutoResizeItemGrid();
    }

    public bool AddItem(Item item)
    {
        foreach (GameObject slot in itemSlots)
        {
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
            if (itemSlot.item == null)
            {
                itemSlot.AcceptItem(item, null);
                return true;
            }
        }
        return false;
    }
    public bool AddItem(Item item, int id)
    {
        ItemSlot itemSlot = itemSlots[id].GetComponent<ItemSlot>();
        if (itemSlot.item == null)
        {
            itemSlot.AcceptItem(item, null);
            return true;
        }
        return false;
    }

    [InspectorButton("AutoResizeItemGrid")]
    public bool inspectorResizeItemGrid;

    void AutoResizeItemGrid()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        // -- Set width/height of the inventory based on chosen rows&columns
        GridLayoutGroup gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
        int width  = ((int)gridLayoutGroup.cellSize.x * columns) + ((int)gridLayoutGroup.spacing.x * (columns - 1));
        int height = ((int)gridLayoutGroup.cellSize.y * rows)    + ((int)gridLayoutGroup.spacing.y * (rows - 1));
        rectTransform.sizeDelta = new Vector2(width, height);

        // -- Offset itemGrid on y, to be equal to distance from parent window edges on x. last subtraction in finalItemGridHeight
        //    is because of pivot points messing up the localPosition. so we have to lower the grid by half parentWindow height.
        int parentWindowWidth   = (int)this.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        int parentWindowHeight  = (int)this.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        int finalItemGridHeight = height/2 + (int)(parentWindowWidth - rectTransform.sizeDelta.x)/2 - parentWindowHeight / 2;
        rectTransform.localPosition = new Vector3(0, finalItemGridHeight, 0);
    }
}