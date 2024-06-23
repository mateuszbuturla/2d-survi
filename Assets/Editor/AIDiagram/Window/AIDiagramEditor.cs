using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AIDiagramEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/AI/AI State Graph")]
    public static void ShowExample()
    {
        var window = GetWindow<AIDiagramEditor>();
        window.titleContent = new GUIContent("AI State Graph");
    }

    private void OnEnable()
    {
        AddGraphView();
    }

    private void AddGraphView()
    {
        AIStateGraph graphView = new AIStateGraph();

        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }
}