using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform Player;
    public AIState[] States;
    private int currentStateIndex = 0;

    void Update()
    {
        if (States.Length > 0)
        {
            States[currentStateIndex].UpdateState(this);
        }
    }

    public void TransitionToState(int newStateIndex)
    {
        if (newStateIndex < States.Length)
        {
            currentStateIndex = newStateIndex;
        }
    }
}