using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : Stamina
{
    public Slider staminaBar;
    public override void OnStart()
    {
        staminaBar.minValue = 0;
        staminaBar.maxValue = maxStamina;
        UpdateStaminaBar();
    }

    public override void HandleStaminaChange()
    {
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = currentStamina;
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
