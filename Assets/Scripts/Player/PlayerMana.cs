using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : Mana
{
    public Slider manaBar;
    public override void OnStart()
    {
        manaBar.minValue = 0;
        manaBar.maxValue = maxMana;
        UpdateManaBar();
    }

    public override void HandleManaChange()
    {
        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        manaBar.value = currentMana;
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.E) && CheckIfHasEnoughMana(12))
        {
            ReduceMana(12);
        }
    }
}
