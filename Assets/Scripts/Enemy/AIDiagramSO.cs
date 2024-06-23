using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIDiagramSONodes
{
    public string ID;
    public Vector2 position;
    public AIDiagramNodeType type;
    public List<string> actionIds = new List<string>();
    public List<string> transitionIds = new List<string>();
    public List<string> decisionIds = new List<string>();
    public List<string> stateIds = new List<string>();
}

[CreateAssetMenu(fileName = "AI Diagram", menuName = "AI/Diagram")]
public class AIDiagramSO : ScriptableObject
{
    public List<AIDiagramSONodes> nodes = new List<AIDiagramSONodes>();
}
