using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(TestData))]
public class TileGridEditor : Editor
{
    private const int previewSize = 64;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TestData tileGrid = (TestData)target;

        if (tileGrid.tiles == null || tileGrid.tiles.Length != 9)
        {
            EditorGUILayout.HelpBox("Invalid number of tiles. It must be exactly 9", MessageType.Error);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tile Grid", EditorStyles.boldLabel);

        float cellSize = EditorGUIUtility.currentViewWidth / 5f;

        for (int row = 0; row < 3; row++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int col = 0; col < 3; col++)
            {
                int index = row * 3 + col;

                tileGrid.tiles[index] = (TileBase)EditorGUILayout.ObjectField(
                    GUIContent.none, tileGrid.tiles[index], typeof(TileBase), false, GUILayout.Width(cellSize), GUILayout.Height(cellSize)
                );

                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect previewRect = new Rect(lastRect.x, lastRect.y + EditorGUIUtility.singleLineHeight, previewSize, previewSize);
                DrawTilePreview(tileGrid.tiles[index], previewRect);

                if (col < 2)
                {
                    GUILayout.Space(5);
                }
            }

            EditorGUILayout.EndHorizontal();

            if (row < 2)
            {
                EditorGUILayout.Space();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawTilePreview(TileBase tile, Rect previewRect)
    {
        if (tile != null)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(tile);
            if (texture != null)
            {
                GUI.DrawTexture(previewRect, texture, ScaleMode.ScaleToFit);
            }
        }
    }
}