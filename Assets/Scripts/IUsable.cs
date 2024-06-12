using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable 
{
    public void PrimaryUseEffect(Player player);
    public abstract void SecondaryUseEffect(Player player);
}
