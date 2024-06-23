using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class AIDiagramTransitionNode : AIDiagramNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        type = AIDiagramNodeType.Transition;
    }

    public override void Draw()
    {
        base.Draw();


        ObjectField choiceTriggerField = AIDiagramHelper.CreateGameObjectField<ScriptableObject>(null, null, cb => { });

        mainContainer.Add(choiceTriggerField);


        Port statePort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));

        statePort.portName = "State";

        ports.Add(statePort);

        outputContainer.Add(statePort);


        Port nextStatePort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(float));

        nextStatePort.portName = "Output";

        ports.Add(nextStatePort);

        outputContainer.Add(nextStatePort);


        Port decisionPort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Multi, typeof(string));

        decisionPort.portName = "Decisions";

        ports.Add(decisionPort);

        outputContainer.Add(decisionPort);


        RefreshExpandedState();
    }
}