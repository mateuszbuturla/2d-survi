using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNotInRange", menuName = "AI/Decisions/PlayerNotInRange")]
public class PlayerNotInRangeDecision : AIDecision
{
    public float range = 10f;

    public override bool Decide(AIController controller)
    {
        if (controller.Player != null)
        {
            return Vector3.Distance(controller.transform.position, controller.Player.position) >= range;
        }
        return false;
    }
}