using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class AIDiagramDecisionNode : AIDiagramNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        type = AIDiagramNodeType.Decision;
    }

    public override void Draw()
    {
        base.Draw();

        ObjectField choiceTriggerField = AIDiagramHelper.CreateGameObjectField<ScriptableObject>(null, null, cb => { });

        mainContainer.Add(choiceTriggerField);

        Port transitionPort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(string));

        transitionPort.portName = "Transition";

        ports.Add(transitionPort);

        inputContainer.Add(transitionPort);

        RefreshExpandedState();
    }
}