using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float CurrentHealth;

    public void ApplyDamage(float value)
    {
        CurrentHealth -= value;
        if (CurrentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Unit Dead");
    }
}
