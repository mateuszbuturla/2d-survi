using UnityEngine;

public abstract class AIDecision : ScriptableObject
{
    public virtual void ResetData(AIController controller) { }
    public virtual void UpdateData(AIController controller) { }
    public abstract bool Decide(AIController controller);
}