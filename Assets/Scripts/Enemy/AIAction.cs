using UnityEngine;

// [CreateAssetMenu(fileName = "New Action", menuName = "AI/Action")]
public class AIAction : ScriptableObject
{
    public virtual void Act(AIController controller) { }
}