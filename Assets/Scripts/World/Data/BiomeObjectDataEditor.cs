using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BiomeObjectData))]
public class BiomeObjectDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BiomeObjectData dynamicSO = (BiomeObjectData)target;

        DrawDefaultInspector();

        // if (dynamicSO.staticCount)
        // {
        //     dynamicSO.count = EditorGUILayout.IntField("Static count", dynamicSO.count);
        // }
        // else
        // {
        //     dynamicSO.numSamplesBeforeRejection = EditorGUILayout.IntField("Number of attempts to place object", dynamicSO.numSamplesBeforeRejection);
        //     dynamicSO.minDistanceBetween = EditorGUILayout.IntField("Min distance between", dynamicSO.minDistanceBetween);
        // }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(dynamicSO);
        }
    }
}