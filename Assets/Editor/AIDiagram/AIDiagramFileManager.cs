using System.Collections;
using System.Collections.Generic;
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

            for (int i = 0; i < node.ports.Count; i++)
            {
                Port port = node.ports[i];
                if (port.connected)
                {
                    // DialogueNodeOption newChoice = new DialogueNodeOption();

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

                    // newChoice.content = choice.content;
                    // newChoice.triggerTag = choice.triggerTag;

                    // nodeToSave.choices.Add(newChoice);
                }
            }

            nodesToSave.Add(nodeToSave);
        }

        AIDiagramSO asset = CreateAIDiagramSO("Assets/ScriptableObjects/AI/Diagrams", loadedFileName);

        asset.nodes = nodesToSave;
    }

    private static AIDiagramSO LoadDialogue(string path, string assetName)
    {
        string fullPath = $"{path}/{assetName}.asset";

        return AssetDatabase.LoadAssetAtPath<AIDiagramSO>(fullPath);
    }


    private static AIDiagramSO CreateAIDiagramSO(string path, string assetName)
    {
        string fullPath = $"{path}/{assetName}.asset";

        AIDiagramSO asset = LoadDialogue(path, assetName);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<AIDiagramSO>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }
}
