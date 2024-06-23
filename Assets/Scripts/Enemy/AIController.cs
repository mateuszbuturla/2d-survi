using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class AITransition
{
    public List<AIDecision> decisions = new List<AIDecision>();
    public string stateId;
}

public class AIController : MonoBehaviour
{
    public Transform Player;
    public AIDiagramSO aIDiagram;
    public AIDiagramSONodes currentState;
    public List<AIAction> currentActions = new List<AIAction>();
    public List<AITransition> currentTransitions = new List<AITransition>();

    void Start()
    {
        AIDiagramSONodes startNode = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Start);

        if (startNode == null)
        {
            Debug.LogError("Missing start node in AI Diagram");
        }

        SetCurrentState(startNode.stateIds[0]);
    }

    private AIAction GetAction(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Action && r.ID == id);

        return AssetDatabase.LoadAssetAtPath<AIAction>(node.scriptableObjectPath);
    }

    private AIDecision GetDecition(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Decision && r.ID == id);

        return AssetDatabase.LoadAssetAtPath<AIDecision>(node.scriptableObjectPath);
    }

    private AITransition GetTransition(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Transition && r.ID == id);

        AITransition transition = new AITransition();
        transition.stateId = node.stateIds[0];

        foreach (string decitionId in node.decisionIds)
        {
            transition.decisions.Add(GetDecition(decitionId));
        }

        return transition;
    }

    private void SetCurrentState(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.State && r.ID == id);

        currentState = node;
        currentActions = new List<AIAction>();

        foreach (string action in node.actionIds)
        {
            currentActions.Add(GetAction(action));
        }

        currentTransitions = new List<AITransition>();

        foreach (string action in node.transitionIds)
        {
            currentTransitions.Add(GetTransition(action));
        }
    }

    void Update()
    {
        foreach (AITransition transition in currentTransitions)
        {
            bool shouldChangeState = true;

            foreach (AIDecision decision in transition.decisions)
            {
                if (!decision.Decide(this))
                {
                    shouldChangeState = false;
                }
            }

            if (shouldChangeState)
            {
                SetCurrentState(transition.stateId);
            }
        }

        foreach (AIAction action in currentActions)
        {
            action.Act(this);
        }
    }
}