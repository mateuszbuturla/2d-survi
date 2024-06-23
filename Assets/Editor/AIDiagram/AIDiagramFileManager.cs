using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class AIDiagramFileManager
{
    public static AIStateGraph graphView;

    private static Dictionary<string, AIDiagramNode> loadedNodes;
    private static List<AIDiagramNode> nodes;

    public static void Initialize(AIStateGraph aiGraphView)
    {
        graphView = aiGraphView;
        loadedNodes = new Dictionary<string, AIDiagramNode>();
        nodes = new List<AIDiagramNode>();
    }

    private static void GetElementsFromGraphView()
    {
        graphView.graphElements.ForEach(graphElement =>
        {
            if (graphElement is AIDiagramNode node)
            {
                nodes.Add(node);
                return;
            }
        });
    }


    public static void Save(string loadedFileName)
    {
        GetElementsFromGraphView();

        List<AIDiagramSONodes> nodesToSave = new List<AIDiagramSONodes>();

        foreach (AIDiagramNode node in nodes)
        {
            AIDiagramSONodes nodeToSave = new AIDiagramSONodes();
            nodeToSave.ID = node.id;
            nodeToSave.position = node.GetPosition().position;
            nodeToSave.type = node.type;

            if (node.scriptableObject)
            {
                nodeToSave.scriptableObjectPath = AssetDatabase.GetAssetPath(node.scriptableObject);
            }

            for (int i = 0; i < node.ports.Count; i++)
            {
                Port port = node.ports[i];
                if (port.connected)
                {
                    // Find the edge that is connected to a specific port
                    graphView.edges.ToList().ForEach(edge =>
                    {
                        if (edge.output == port && edge.input != null)
                        {
                            AIDiagramNode inputNode = (AIDiagramNode)edge.input.node;
                            string nodeId = inputNode.id;

                            switch (inputNode.type)
                            {
                                case AIDiagramNodeType.Action:
                                    nodeToSave.actionIds.Add(nodeId);
                                    break;
                                case AIDiagramNodeType.State:
                                    nodeToSave.stateIds.Add(nodeId);
                                    break;
                                case AIDiagramNodeType.Transition:
                                    nodeToSave.transitionIds.Add(nodeId);
                                    break;
                                case AIDiagramNodeType.Decision:
                                    nodeToSave.decisionIds.Add(nodeId);
                                    break;
                            }
                        }
                    });
                }
            }

            nodesToSave.Add(nodeToSave);
        }

        AIDiagramSO asset = CreateAIDiagramSO("Assets/ScriptableObjects/AI/Diagrams", loadedFileName);

        asset.nodes = nodesToSave;
    }

    private static AIDiagramSO LoadDiagram(string path, string assetName)
    {
        string fullPath = $"{path}/{assetName}.asset";

        return AssetDatabase.LoadAssetAtPath<AIDiagramSO>(fullPath);
    }


    private static AIDiagramSO CreateAIDiagramSO(string path, string assetName)
    {
        string fullPath = $"{path}/{assetName}.asset";

        AIDiagramSO asset = LoadDiagram(path, assetName);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<AIDiagramSO>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }

    //LOAD

    public static string Load(string filePath)
    {
        string file = Path.GetFileNameWithoutExtension(filePath);

        AIDiagramSO data = LoadDiagram("Assets/ScriptableObjects/AI/Diagrams", file);

        if (data == null)
        {
            Debug.LogError("Could not find the file!");

            return "";
        }

        LoadNodes(data.nodes);
        LoadNodesConnections(data.nodes);

        return file;
    }

    private static void LoadNodes(List<AIDiagramSONodes> nodes)
    {
        foreach (AIDiagramSONodes nodeData in nodes)
        {
            AIDiagramNode node = graphView.CreateNode(nodeData.type, nodeData.position, false);

            node.id = nodeData.ID;
            node.type = nodeData.type;

            if (nodeData.scriptableObjectPath != null && nodeData.scriptableObjectPath.Length > 0)
            {
                node.scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(nodeData.scriptableObjectPath);
            }

            node.Draw();

            loadedNodes.Add(node.id, node);

            graphView.AddElement(node);
        }
    }

    private static void LoadNodesConnections(List<AIDiagramSONodes> nodes)
    {
        foreach (var vkp in loadedNodes)
        {
            AIDiagramSONodes node = nodes.Find(r => r.ID == vkp.Value.id);

            ConnectNodes(vkp.Value, node.actionIds, loadedNodes, typeof(int));
            ConnectNodes(vkp.Value, node.transitionIds, loadedNodes, typeof(bool));
            ConnectNodes(vkp.Value, node.decisionIds, loadedNodes, typeof(string));
            ConnectNodes(vkp.Value, node.stateIds, loadedNodes, typeof(float));
        }
    }

    private static void ConnectNodes(AIDiagramNode node, List<string> targetIds, Dictionary<string, AIDiagramNode> loadedNodes, Type portType)
    {
        foreach (string targetId in targetIds)
        {
            AIDiagramNode targetNode = loadedNodes.FirstOrDefault(n => n.Value.id == targetId).Value;

            if (targetNode != null)
            {
                Port outputPort = GetOutputPort(node, portType);
                Port inputPort = GetInputPort(targetNode, portType);

                Edge edge = outputPort.ConnectTo(inputPort);

                graphView.AddElement(edge);

                RefreshPorts(node);
            }
        }
    }

    private static Port GetOutputPort(AIDiagramNode node, Type portType)
    {
        return node.outputContainer.Children().OfType<Port>()
        .FirstOrDefault(p => p.portType == portType) as Port;
    }

    private static Port GetInputPort(AIDiagramNode node, Type portType)
    {
        return node.inputContainer.Children().OfType<Port>()
        .FirstOrDefault(p => p.portType == portType) as Port;
    }

    private static void RefreshPorts(AIDiagramNode node)
    {
        node.RefreshPorts();
    }
}
