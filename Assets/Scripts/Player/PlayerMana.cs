using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : Mana
{
    public UIBar manaBar;
    public override void OnStart()
    {
        manaBar.SetupBar(0, maxMana, currentMana);
        UpdateManaBar();
    }

    public override void HandleManaChange()
    {
        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        manaBar.UpdateValue(currentMana);
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
