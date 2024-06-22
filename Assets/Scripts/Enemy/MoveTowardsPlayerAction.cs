using UnityEngine;

[CreateAssetMenu(fileName = "MoveTowardsPlayer", menuName = "AI/Actions/MoveTowardsPlayer")]
public class MoveTowardsPlayerAction : AIAction
{
    public float speed = 3f;

    public override void Act(AIController controller)
    {
        if (controller.Player != null)
        {
            Vector3 direction = (controller.Player.position - controller.transform.position).normalized;
            controller.transform.position += direction * speed * Time.deltaTime;
        }
    }
}