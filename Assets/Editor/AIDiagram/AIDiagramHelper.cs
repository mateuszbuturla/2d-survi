using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
public class AIDiagramHelper : MonoBehaviour
{
    public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null, bool small = false)
    {
        TextField textField = new TextField()
        {
            value = value,
            label = label,
            style = { width = 250 }
        };

        if (small)
        {
            textField = new TextField()
            {
                value = value,
                label = label,
                style = { width = 30 }
            };
        }

        if (onValueChanged != null)
        {
            textField.RegisterValueChangedCallback(onValueChanged);
        }

        return textField;
    }

    public static Toggle CreateBoolField(bool value = false, string label = null, EventCallback<ChangeEvent<bool>> onValueChanged = null)
    {
        Toggle toggleField = new Toggle()
        {
            value = value,
            label = label,
            style = { width = 250 }
        };

        if (onValueChanged != null)
        {
            toggleField.RegisterValueChangedCallback(onValueChanged);
        }

        return toggleField;
    }

    public static ObjectField CreateGameObjectField<T>(Object value = null, string label = null, EventCallback<ChangeEvent<Object>> onValueChanged = null, bool allowSceneObjects = false)
    {
        ObjectField objectField = new ObjectField()
        {
            value = value,
            label = label,
            objectType = typeof(T),
            allowSceneObjects = allowSceneObjects
        };

        if (onValueChanged != null)
        {
            objectField.RegisterValueChangedCallback(onValueChanged);
        }

        return objectField;
    }
}