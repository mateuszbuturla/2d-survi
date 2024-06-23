using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform Player;
    public AIDiagram aIDiagram;
    private AIState currentState;

    void Start()
    {
        currentState = aIDiagram.mainState;
    }

    void Update()
    {
        if (aIDiagram != null && currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void TransitionToState(AIState aIState)
    {
        currentState = aIState;
    }
}