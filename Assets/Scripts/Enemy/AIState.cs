using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "AI/State")]
public class AIState : ScriptableObject
{
    public AIAction[] Actions;
    public Transition[] Transitions;

    public void UpdateState(AIController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(AIController controller)
    {
        foreach (var action in Actions)
        {
            action.Act(controller);
        }
    }

    private void CheckTransitions(AIController controller)
    {
        foreach (var transition in Transitions)
        {
            if (transition.Decision.Decide(controller))
            {
                controller.TransitionToState(transition.TrueStateIndex);
                return;
            }
        }
    }
}