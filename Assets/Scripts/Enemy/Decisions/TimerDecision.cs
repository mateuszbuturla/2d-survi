using UnityEngine;

[CreateAssetMenu(fileName = "TimerDecision", menuName = "AI/Decisions/Timer")]
public class TimerDecision : AIDecision
{

    public float time;
    public float timer;

    public override void ResetData(AIController controller)
    {
        timer = time;
    }

    public override void UpdateData(AIController controller)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }


    public override bool Decide(AIController controller)
    {
        return timer <= 0;
    }
}
