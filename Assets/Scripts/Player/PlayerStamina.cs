using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : Stamina
{
    public UIBar staminaBar;
    public override void OnStart()
    {
        staminaBar.SetupBar(0, maxStamina, currentStamina);
        UpdateStaminaBar();
    }

    public override void HandleStaminaChange()
    {
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        staminaBar.UpdateValue(currentStamina);
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.R) && CheckIfHasEnoughStamina(5))
        {
            ReduceStamina(5);
        }
    }
}
