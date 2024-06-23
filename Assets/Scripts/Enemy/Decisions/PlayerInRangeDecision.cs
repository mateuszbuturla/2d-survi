using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInRangeDecision", menuName = "AI/Decisions/PlayerInRange")]
public class PlayerInRangeDecision : AIDecision
{
    public float range = 10f;

    public override bool Decide(AIController controller)
    {
        if (controller.Player != null)
        {
            return Vector3.Distance(controller.transform.position, controller.Player.position) <= range;
        }
        return false;
    }
}