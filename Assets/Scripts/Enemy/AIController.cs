using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<string, AIDecision> allAIDecisions = new Dictionary<string, AIDecision>();

    void Start()
    {
        aIDiagram = Instantiate(aIDiagram);
        AIDiagramSONodes startNode = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Start);

        if (startNode == null)
        {
            Debug.LogError("Missing start node in AI Diagram");
        }

        foreach (AIDiagramSONodes node in aIDiagram.nodes.Where(r => r.type == AIDiagramNodeType.Decision))
        {
            Debug.Log(node.scriptableObjectPath);
            allAIDecisions[node.ID] = GetDecition(node.ID);
        }

        SetCurrentState(startNode.stateIds[0]);
    }

    private AIAction GetAction(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Action && r.ID == id);

        return Instantiate(AssetDatabase.LoadAssetAtPath<AIAction>(node.scriptableObjectPath));
    }

    private AIDecision GetDecition(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Decision && r.ID == id);

        return Instantiate(AssetDatabase.LoadAssetAtPath<AIDecision>(node.scriptableObjectPath));
    }

    private AITransition GetTransition(string id)
    {
        AIDiagramSONodes node = aIDiagram.nodes.Find(r => r.type == AIDiagramNodeType.Transition && r.ID == id);

        AITransition transition = new AITransition();
        transition.stateId = node.stateIds[0];

        foreach (string decitionId in node.decisionIds)
        {
            transition.decisions.Add(allAIDecisions[decitionId]);
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
        foreach (var vkp in allAIDecisions)
        {
            vkp.Value.UpdateData(this);
        }

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

                foreach (AIDecision decisionId in transition.decisions)
                {
                    decisionId.ResetData(this);
                }
            }
        }

        foreach (AIAction action in currentActions)
        {
            action.Act(this);
        }
    }
}