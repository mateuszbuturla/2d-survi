using UnityEngine;

public class Mana : MonoBehaviour
{
    public float currentMana;
    public int maxMana;
    public int pasiveManaResoreValue;

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        if (currentMana < maxMana)
        {
            RestoreMana(pasiveManaResoreValue * Time.deltaTime);
        }
        OnUpdate();
    }

    public bool ReduceMana(int amount)
    {
        if (currentMana < amount)
        {
            return false;
        }

        currentMana -= amount;
        HandleManaChange();

        return true;
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

    public virtual void OnStart() { }
    public virtual void OnUpdate() { }

    public virtual void HandleManaChange() { }
}
