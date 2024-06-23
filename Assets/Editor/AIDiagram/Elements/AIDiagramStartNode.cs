using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AIDiagramStartNode : AIDiagramNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        type = AIDiagramNodeType.Start;
    }

    public override void Draw()
    {
        base.Draw();

        Port nextStatePort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(float));

        nextStatePort.portName = "Output";

        ports.Add(nextStatePort);

        outputContainer.Add(nextStatePort);

        RefreshExpandedState();
    }
}
