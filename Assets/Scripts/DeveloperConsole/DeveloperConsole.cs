using System;
using System.Reflection;
using TMPro;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
    public static DeveloperConsole instance;
    void Awake() { if (instance == null) { instance = this; } }

    private bool isActive = false;
    public GameObject developerConsoleObject;
    public TMP_InputField inputField;
    public GameObject developerConsoleMessagePrefab;
    public GameObject developerConsoleLogsContainer;

    private void Start()
    {
        developerConsoleObject.SetActive(isActive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            isActive = !isActive;
            developerConsoleObject.SetActive(isActive);
        }
    }

    public void ParseConsoleInput()
    {
        string input = inputField.text;

        Type type = typeof(DeveloperConsole);

        try
        {
            string[] inputs = input.Split(" ", 2);

            MethodInfo methodInfo = type.GetMethod(inputs[0]);
            if (methodInfo == null) { CreateConsoleMessage(new DeveloperConsoleLog("Command not found", DeveloperConsoleLogType.ERROR)); }

            ParameterInfo[] parameters = methodInfo.GetParameters();
            object classInstance = this;

            if (parameters.Length == 0)
            {
                methodInfo.Invoke(classInstance, null);
            }
            else
            {
                object[] parametersArray = inputs[1..];
                methodInfo.Invoke(classInstance, parametersArray);
            }

            inputField.text = "";

        }
        catch (Exception e)
        {

        }
    }

    public void CreateDroppedItem(string item)
    {
        CreateConsoleMessage(new DeveloperConsoleLog("Item created"));
        GameObject droppedItem = Instantiate(Singleton.instance.droppedItemPrefab);
        GameObject itemObject = AllItems.GetItem(item);
        itemObject.transform.SetParent(droppedItem.transform);

        droppedItem.GetComponent<DroppedItem>().item = itemObject;
        droppedItem.transform.position = Singleton.instance.players[0].transform.position;
        droppedItem.GetComponent<DroppedItem>().RefreshSprite();
    }

    // -- Needs casts in reflection to int
    //public void CreateDroppedItemAt(string item, int X, int Y)
    //{
    //    GameObject droppedItem = Instantiate(Singleton.instance.droppedItemPrefab);
    //    GameObject itemObject = Instantiate(AllItems.GetItem(item));
    //    itemObject.transform.SetParent(droppedItem.transform);

    //    droppedItem.GetComponent<DroppedItem>().item = itemObject;
    //    droppedItem.GetComponent<DroppedItem>().RefreshSprite();

    //    CreateConsoleMessage("Item created.");
    //}

    private void CreateConsoleMessage(DeveloperConsoleLog log)
    {
        GameObject consoleMessage = Instantiate(developerConsoleMessagePrefab);
        consoleMessage.GetComponent<TextMeshProUGUI>().text = log.message;
        consoleMessage.GetComponent<TextMeshProUGUI>().color = log.color;
        consoleMessage.transform.SetParent(developerConsoleLogsContainer.transform);
    }

    public void Test()
    {
        CreateConsoleMessage(new DeveloperConsoleLog("Test"));
    }

    public void AddFishingRod()
    {
        CreateConsoleMessage(new DeveloperConsoleLog("Fishing rod has been added"));
        Singleton.instance.players[0].GetComponent<Player>().playerInventory.AddItem(AllItems.GetItemComponent("Wooden Fishing Rod"));
    }
}