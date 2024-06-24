using UnityEngine;

[CreateAssetMenu(fileName = "DebugLogAction", menuName = "AI/Actions/DebugLog")]
public class DebugLogAction : AIAction
{
    public string message;

    public override void Act(AIController controller)
    {
        Debug.Log(message);
    }
}