using UnityEngine;

public class FishingRodItem : Useable
{
    public override void PrimaryUseEffect(Player player)
    {
        player.fishingController.HandleFishingInput();
    }

    public override void SecondaryUseEffect(Player player)
    {

    }
}
