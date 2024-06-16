using UnityEngine;

public class Mana : MonoBehaviour
{
    public float maxMana;
    public float currentMana;
    public int pasiveManaResoreValue;

    public virtual void Start()
    {
        currentMana = maxMana;
        OnStart();
    }

    public virtual void Update()
    {
        if (currentMana < maxMana)
        {
            RestoreMana(pasiveManaResoreValue * Time.deltaTime);
        }
    }

    public void ReduceMana(int amount)
    {
        currentMana -= amount;
        HandleManaChange();
    }

    public void RestoreMana(float amount)
    {
        currentMana += amount;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        HandleManaChange();
    }

    public bool CheckIfHasEnoughMana(int amount)
    {
        return amount <= currentMana;
    }

    public virtual void OnStart() { }
    public virtual void HandleManaChange() { }
}
