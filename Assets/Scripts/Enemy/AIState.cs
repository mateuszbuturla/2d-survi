using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "AI/State")]
public class AIState : ScriptableObject
{
    public AIAction[] actions;
    public Transition[] transitions;

    public void UpdateState(AIController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(AIController controller)
    {
        foreach (var action in actions)
        {
            action.Act(controller);
        }
    }

    private void CheckTransitions(AIController controller)
    {
        foreach (var transition in transitions)
        {
            bool shouldChangeState = true;
            foreach (AIDecision decision in transition.decisions)
            {
                if (!decision.Decide(controller))
                {
                    shouldChangeState = false;
                }
            }

            if (shouldChangeState)
            {
                controller.TransitionToState(transition.state);
                return;
            }
        }
    }
}