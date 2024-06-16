using System;
using System.Collections;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
    public static DeveloperConsole instance;
    void Awake() { if (instance == null) { instance = this; } }

    public GameObject inputField;
    public GameObject developerConsoleMessagePrefab;


    public void ParseConsoleInput()
    {
        string input = inputField.GetComponent<TMP_InputField>().text;

        Type type = typeof(DeveloperConsole);

        try
        {
            string[] inputs = input.Split(" ");

            MethodInfo methodInfo = type.GetMethod(inputs[0]);
            if (methodInfo == null) { CreateConsoleMessage("Incorrect console message.");}
            
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
            
        }
        catch (Exception e)
        {
            //e.Describe();
            print(e.StackTrace);
            print(e.Message);
        }
    }

    public void CreateDroppedItem(string item)
    {
        GameObject droppedItem = Instantiate(Singleton.instance.droppedItemPrefab);
        AllItems.GetItem(item, ref droppedItem.GetComponent<DroppedItem>().item);
        droppedItem.GetComponent<DroppedItem>().RefreshSprite();

        CreateConsoleMessage("Item created.");
    }

    private void CreateConsoleMessage(string messageText)
    {
        if (!this.gameObject.activeSelf) { return; }
        GameObject consoleMessage = Instantiate(developerConsoleMessagePrefab);
        consoleMessage.GetComponent<TextMeshProUGUI>().text = messageText;
        consoleMessage.transform.SetParent(this.gameObject.transform);
        consoleMessage.transform.SetSiblingIndex(2);
    }
}