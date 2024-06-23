using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEditor;

public class AIStateGraph : GraphView
{
    public string loadedFileName = "";

    public AIStateGraph()
    {
        AddManipulators();
        AddGridBackground();
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort == port || startPort.node == port.node || startPort.direction == port.direction)
            {
                return;
            }

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }



    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(CreateNodeContextualMenu("Create start node", AIDiagramNodeType.Start));
        this.AddManipulator(CreateNodeContextualMenu("Create state node", AIDiagramNodeType.State));
        this.AddManipulator(CreateNodeContextualMenu("Create action node", AIDiagramNodeType.Action));
        this.AddManipulator(CreateNodeContextualMenu("Create transition node", AIDiagramNodeType.Transition));
        this.AddManipulator(CreateNodeContextualMenu("Create decision node", AIDiagramNodeType.Decision));
        this.AddManipulator(CreateSaveContextealMenu("Save AI diagram"));
        this.AddManipulator(CreateLoadContextealMenu("Load AI diagram"));
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }

    public AIDiagramNode CreateNode(AIDiagramNodeType type, Vector2 position, bool isNewCreated = true)
    {
        Type nodeType = Type.GetType($"AIDiagram{type}Node");
        AIDiagramNode node = (AIDiagramNode)Activator.CreateInstance(nodeType);

        node.Initialize(position);

        if (isNewCreated)
        {
            node.Draw();
        }

        return node;
    }

    private void TryToCreateNode(AIDiagramNodeType type, Vector2 position)
    {
        AddElement(CreateNode(type, position));
    }

    private IManipulator CreateNodeContextualMenu(string actionName, AIDiagramNodeType type)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionName, action => TryToCreateNode(type, action.eventInfo.localMousePosition))
        );

        return contextualMenuManipulator;
    }

    private IManipulator CreateSaveContextealMenu(string actionName)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionName, action => Save())
        );

        return contextualMenuManipulator;
    }

    private IManipulator CreateLoadContextealMenu(string actionName)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionName, action => Load())
        );

        return contextualMenuManipulator;
    }

    private void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();

        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }

    private void ClearGraph()
    {
        graphElements.ForEach(graphElement => RemoveElement(graphElement));
    }

    private void Save()
    {
        AIDiagramFileManager.Initialize(this);
        AIDiagramFileManager.Save(loadedFileName);
    }

    private void Load()
    {
        string filePath = EditorUtility.OpenFilePanel("AI Diagram Data", "Assets/ScriptableObjects/AI/Diagrams", "asset");

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        ClearGraph();
        AIDiagramFileManager.Initialize(this);
        loadedFileName = AIDiagramFileManager.Load(filePath);
    }
}