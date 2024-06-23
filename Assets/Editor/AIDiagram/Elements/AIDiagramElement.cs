using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Numerics;
using UnityEngine;
using System.Collections.Generic;

public class AIDiagramNode : Node
{
    public string id { get; set; }
    public string speakingPerson { get; set; }
    public string content { get; set; }
    public List<Port> ports { get; set; }
    public AIDiagramNodeType type;

    public virtual void Initialize(UnityEngine.Vector2 position)
    {
        id = System.Guid.NewGuid().ToString();
        ports = new List<Port>();

        SetPosition(new Rect(position, UnityEngine.Vector2.zero));
    }

    public virtual void Draw()
    {
        // if (type != DialogueType.Start)
        // {
        //     Port inputPort = DialogueElementHelper.CreateInputPort(this);
        //     inputContainer.Add(inputPort);
        // }

        VisualElement customDataContainer = new VisualElement();

        extensionContainer.Add(customDataContainer);
    }
}
