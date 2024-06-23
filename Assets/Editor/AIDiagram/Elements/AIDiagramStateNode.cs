using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class AIDiagramStateNode : AIDiagramNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        type = AIDiagramNodeType.State;
    }

    public override void Draw()
    {
        base.Draw();

        ObjectField choiceTriggerField = AIDiagramHelper.CreateGameObjectField<ScriptableObject>(null, null, cb => { });

        mainContainer.Add(choiceTriggerField);


        Port transitionInput = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(float));

        transitionInput.portName = "Transition input";

        ports.Add(transitionInput);

        outputContainer.Add(transitionInput);




        Port actionPort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Multi, typeof(int));

        actionPort.portName = "Action";

        ports.Add(actionPort);

        outputContainer.Add(actionPort);



        Port transitionPort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Multi, typeof(bool));

        transitionPort.portName = "Transitions output";

        ports.Add(transitionPort);

        outputContainer.Add(transitionPort);


        RefreshExpandedState();
    }
}
