using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;
    public int pasiveStaminaResoreValue;

    public virtual void Start()
    {
        currentStamina = maxStamina;
        OnStart();
    }

    public virtual void Update()
    {
        if (currentStamina < maxStamina)
        {
            RestoreStamina(pasiveStaminaResoreValue * Time.deltaTime);
        }
    }

    public void ReduceStamina(int amount)
    {
        currentStamina -= amount;
        HandleStaminaChange();
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        HandleStaminaChange();
    }

    public bool CheckIfHasEnoughStamina(int amount)
    {
        return amount <= currentStamina;
    }

    public virtual void OnStart() { }
    public virtual void HandleStaminaChange() { }
}
