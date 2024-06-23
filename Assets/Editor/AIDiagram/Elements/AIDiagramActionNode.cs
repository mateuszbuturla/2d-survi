using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class AIDiagramActionNode : AIDiagramNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        type = AIDiagramNodeType.Action;
    }

    public override void Draw()
    {
        base.Draw();

        ObjectField choiceTriggerField = AIDiagramHelper.CreateGameObjectField<ScriptableObject>(scriptableObject, null, cb => scriptableObject = cb.newValue as ScriptableObject);

        mainContainer.Add(choiceTriggerField);

        Port statePort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(int));

        statePort.portName = "State";

        ports.Add(statePort);

        inputContainer.Add(statePort);

        RefreshExpandedState();
    }
}