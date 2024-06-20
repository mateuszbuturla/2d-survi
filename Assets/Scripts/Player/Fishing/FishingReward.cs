using UnityEngine;

public class FishingReward : MonoBehaviour
{
    public void ShowReward(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
